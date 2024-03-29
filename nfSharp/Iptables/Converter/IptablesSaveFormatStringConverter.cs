//
//  Copyright (C) 2010 SharpKnocking project
//  File created by mangelp
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using NFSharp.Iptables.Core;
using System.Text;
using NFSharp.Iptables.Core.Extensions.Match;
using NFSharp.Iptables.Core.Commands.Options;

namespace NFSharp.Iptables.Converter {

    /// <summary>
    /// Converts a set of rules into an string
    /// </summary>
    public class IptablesSaveFormatStringConverter : IRuleConverter<String> {
        public IptablesSaveFormatStringConverter () {
        }

        public String Convert (NetfilterTableSet tableSet) {
            StringBuilder sb = new StringBuilder(1024);

            NetfilterTable[] tables = tableSet.Tables;

            if (tables.Length == 1) {
                this.ConvertTable(tables[0], sb);
            } else if (tables.Length > 1) {
                this.ConvertTable(tables[0], sb);
                for(int i=1; i < tables.Length; i++) {
                    sb.Append("\n");
                    this.ConvertTable(tables[0], sb);
                }
            }

            return "#Generated by "+this.GetType().FullName+" on "+
                   DateTime.Now.ToShortDateString() + " " +
                   DateTime.Now.ToShortTimeString() + "\n" +
                   sb.ToString() +
                   "\nCOMMIT\n" + "#Completed on " +
                   DateTime.Now.ToShortDateString() + " " +
                   DateTime.Now.ToShortTimeString();
        }

        protected void ConvertTable(NetfilterTable table, StringBuilder sb) {
            // Start with the table name in lowercase
            sb.Append("*" + table.Type.ToString().ToLower());
            NetfilterChain[] chains = table.Chains;
            // Then add the strings for each chain that will contain all the
            // rules in these chains
            for(int i=0; i < chains.Length; i++) {
                sb.Append("\n" + this.GetChainDefinition(chains[i]));
            }

            NetfilterChain chain = null;

            for(int i=0; i < chains.Length; i++) {
                chain = (NetfilterChain)chains[i];

                if(chain.Rules.Count > 0) {
                    sb.Append('\n');
                    this.ConvertChain(chain, sb);
                }
            }
        }

        protected string GetChainDefinition(NetfilterChain chain) {
            string chainSpec = ":";

            // If is builtIn it must have a default target, but it is user
            // defined there is no target and is specified as -
            if(chain.IsBuiltIn) {
                chainSpec += chain.ChainType.ToString().ToUpper()+ " "
                             + chain.DefaultTarget.ToString().ToUpper();
            } else {
                chainSpec += chain.Name + " -";
            }

            chainSpec += " [" + chain.PacketCount + ":" + chain.ByteCount + "]";

            return chainSpec;
        }

        protected void ConvertChain(NetfilterChain chain, StringBuilder sb) {
            if(chain.Rules.Count == 0) {
                return;
            }

            // We simply have to add the -A with the chain name before each rule
            // to have it added to the chain by iptables-save
            sb.Append("-A " + chain.CurrentName + " ");

            sb.Append(chain.Rules[0].ToString());

            for(int i = 1; i < chain.Rules.Count; i++) {
                sb.Append("\n-A " + chain.CurrentName + " ");

                this.ConvertRule(chain.Rules[i], sb);
            }
        }

        protected void ConvertRule(NetfilterRule rule, StringBuilder sb) {
            // Console.WriteLine("** Converting rule to string ** ");
			JumpOption jumpOption = null;

            for(int i=0; i<rule.Options.Count; i++) {
				GenericOption option = rule.Options[i];

                if(option is JumpOption) {
					jumpOption = (JumpOption) option;
                    continue;
                } else if(rule.Options[i] is MatchExtensionOption) {
                    // First we print the option and then the parameters
                    sb.Append(option.ToString() + " ");
                    MatchExtensionHandler handler = this.FindMatchExtensionHandler((MatchExtensionOption)this.options[i]);
                    if(handler!=null) {
                        handler.AppendContentsTo(sb);
                        sb.Append(" ");
                    }
                    // Console.WriteLine("MatchExtension: "+this.options[i]+" "+handler);
                } else {
                    // Console.WriteLine("Option: "+this.options[i]);
                    sb.Append(option.ToString() + " ");
                }
            }

            // Console.WriteLine("Target extension: "+jumpOption);
            if(jumpOption != null) {
                sb.Append(jumpOption.ToString());
            }
        }
    }
}
