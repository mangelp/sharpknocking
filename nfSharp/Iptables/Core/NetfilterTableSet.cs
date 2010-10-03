// NetfilterTableSet.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp@gmail.com
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
//
//

using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

using NFSharp.Iptables.Core.Commands;

namespace NFSharp.Iptables.Core {
    /// <summary>
    /// This class models the entire set of existing tables in netfilter.
    /// </summary>
    /// <remarks>
    /// This is the entry point to the functionality of this library. This class
    /// lets you alter the rules in the tables commiting the changes over the
    /// real ones or load them for further management.
    /// </remarks>
    public class NetfilterTableSet {
        private List<NetfilterTable> tables;

        /// <summary>
        /// Array with all the tables found
        /// </summary>
        public NetfilterTable[] Tables {
            get {
                return this.tables.ToArray();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>
        /// This constructor creates the default filter table as iptables does.
        /// </remarks>
        public NetfilterTableSet() {
            tables = new List<NetfilterTable>();
            this.InitEmptySet();
        }

        /// <summary>
        /// Inits the rule set with the default filter table named filter and with
        /// the default chains INPUT, OUTPUT and FORWARD.
        /// </summary>
        protected virtual void InitEmptySet() {
            this.tables.Clear();
            //Add a new filter table.
            NetfilterTable table = new NetfilterTable();
            this.tables.Add(table);
        }

        /// <summary>
        /// Adds a new table to the table set
        /// </summary>
        /// <param name="table">
        /// A <see cref="NetfilterTable"/>
        /// </param>
        public void AddTable(NetfilterTable table) {
            this.tables.Add(table);
        }

        /// <summary>
        /// Finds a chain in the current set of tables
        /// </summary>
        /// <remarks>
        /// This relies in the method FindChain(name) defined in the class NetfilterChain
        /// </remarks>
        /// <returns>
        /// The found chain or null
        /// </returns>
        public NetfilterChain FindChain(string name) {
            NetfilterChain result = null;

            foreach(NetfilterTable table in this.tables) {
                result = table.FindChain(name);

                if(result!=null) {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an instance of the default table named filter.
        /// </summary>
        /// <returns>
        /// The default table or null if it doesn't exists
        /// </returns>
        public NetfilterTable GetDefaultTable() {
            for(int i=0; i<this.tables.Count; i++) {
                if((this.tables[i]).Type == PacketTableType.Filter) {
                    return this.tables[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Removes all the information in the tables
        /// </summary>
        public void Clear() {
            foreach(NetfilterTable table in this.tables) {
                table.Clear();
            }
            this.tables.Clear();
        }

        /// <summary>
        /// Returns the current rule set separating each item with the unix
        /// new line character.
        /// </summary>
        public override string ToString() {
            return this.GetType().FullName + "@[Tables:" + this.tables.Count + "]";
        }


        /// <summary>
        /// Simulates the execution of the rule over the current table set.
        /// </summary>
        /// <remarks>
        /// Currently the actions are limited to those that changes chains or rules and
        /// that are executed over the filter table.
        /// </remarks>
        /// <param name="cmd">Command to exec with the rule</param>
        public void Exec(GenericCommand cmd) {
            NetfilterTable table = this.GetDefaultTable();
            NetfilterChain chain = null;
            int pos = 0;

            switch(cmd.CommandType) {
            case RuleCommands.NewChain:
                //Creates the chain.
                chain = new NetfilterChain(table, cmd.ChainName);
                table.AddChain(chain);
                break;
            case RuleCommands.FlushChain:
                //Clears all the rules from the chain
                chain = table.FindChain(cmd.ChainName);
                if(chain==null)
                    throw new InvalidOperationException("The chain "+
                                                        cmd.ChainName+
                                                        " doesn't exist in table "+table.Type);
                chain.Rules.Clear();
                break;
            case RuleCommands.DeleteChain:
                //Removes a chain if it is empty and is not a built-in one
                pos = table.IndexOfChain(cmd.ChainName);
                if(pos>-1) {
                    chain = table.Chains[pos];
                }

                if(chain==null)
                    throw new InvalidOperationException("The chain "+
                                                        cmd.ChainName+
                                                        " doesn't exist in table "+table.Type);
                else if(chain.IsBuiltIn)
                    throw new InvalidOperationException("Can't delete "+
                                                        " built-in chain "+cmd.ChainName);
                else if(chain.Rules.Count>0)
                    throw new InvalidOperationException("Can't delete "+
                                                        " chain "+cmd.ChainName+
                                                        ". Its not empty");

                table.RemoveChain(pos);
                break;
            case RuleCommands.AppendRule:
                //Adds a rule to a chain
                chain = table.FindChain(cmd.ChainName);

                if(chain==null)
                    throw new InvalidOperationException("The chain "+
                                                        cmd.ChainName+
                                                        " doesn't exist in table "+table.Type);

                chain.Rules.Add(cmd.Rule);
                break;
            case RuleCommands.DeleteRule:
                //Removes a rule from a chain
                chain = table.FindChain(cmd.ChainName);

                if(chain==null)
                    throw new InvalidOperationException("The chain "+
                                                        cmd.ChainName+
                                                        " doesn't exist in table "+table.Type);

                DeleteRuleCommand delCmd = (DeleteRuleCommand)cmd;
                chain.Rules.RemoveAt(delCmd.RuleNum-1);
                break;
            case RuleCommands.InsertRule:
                //Inserts a rule into a chain
                chain = table.FindChain(cmd.ChainName);

                if(chain==null)
                    throw new InvalidOperationException("The chain "+
                                                        cmd.ChainName+
                                                        " doesn't exist in table "+table.Type);

                InsertRuleCommand insCmd = (InsertRuleCommand)cmd;
                chain.Rules.Insert(insCmd.RuleNum-1, cmd.Rule);
                break;
            default:
                //Every other thing is not implemented and must not be used.
                throw new NotImplementedException("Not implemented. Sorry!");
            }

        }
    }
}
