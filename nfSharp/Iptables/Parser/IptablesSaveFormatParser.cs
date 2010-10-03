//
//  Copyright (C) 2009 SharpKnocking project
//  File created by Miguel Angel Perez
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

using NFSharp.Iptables;
using NFSharp.Iptables.Util;
using NFSharp.Iptables.Core;
using NFSharp.Iptables.Core.Commands;
using NFSharp.Iptables.Parser.IptablesSaveFormat;

using CommonUtilities.Types;

namespace NFSharp.Iptables.Parser {
    public class IptablesSaveFormatParser : TextFileParser {
        private NetfilterTable lastFoundTable;
        private NetfilterChain lastFoundChain;
        private TableParser tableParser;
        private CommandParser cmdParser;
        private ChainParser chainParser;

        public IptablesSaveFormatParser(string fileName) :
        base(fileName) {

            this.init();
        }

        public IptablesSaveFormatParser() :
        base( ) {

            this.init();
        }

        protected virtual void init() {
            this.reset();
        }

        public virtual void reset() {
            this.lastFoundChain = null;
            this.lastFoundTable = null;
            this.chainParser = new ChainParser();
            this.cmdParser = new CommandParser();
            this.tableParser = new TableParser();
        }

        protected override void parseLine (string line, NetfilterTableSet tableSet) {
            // No line, no parsing
            if (line == null) {
                return;
            }

            // Remove separators
            line = line.Trim();

            // If line is empty or if it starts with the comment character '#' or
            // if it is the COMMIT token we are finished. COMMIT means the end of
            // the parsing, the rest is just ignored
            if (line.Length == 0 ||
                    line.Equals(""+Tokens.COMMIT, StringComparison.InvariantCultureIgnoreCase) ||
                    line.StartsWith("#")) {

                return;
            }

            PacketTableType tableType;

            if (NetfilterTable.TryGetTableType(line, out tableType)) {
                // The line is a table definition, keep a reference to it
                lastFoundTable = new NetfilterTable(tableType);
                tableSet.AddTable(lastFoundTable);
            } else if (chainParser.IsChain(line)) {
                // The line is a chain definition

                if(lastFoundTable == null) {
                    // If there is no table, cry
                    throw new RuleParserException("Bad format for input string. " +
                                                  "Can't be a chain before the table");
                }

                // We got a chain, we append it to the last added table if it is not a built-in one
                // If it is built-in we already have it
                lastFoundChain = chainParser.ParseChain(line, lastFoundTable);

                if(!lastFoundChain.IsBuiltIn) {
                    lastFoundTable.AddChain(lastFoundChain);
                } else {
                    lastFoundTable.SetChainCounters(lastFoundChain.ChainType, lastFoundChain.PacketCount, lastFoundChain.ByteCount);
                }
            } else if (cmdParser.CanBeACommand(line)) {

                if(lastFoundChain == null) {
                    // No chain was found. Can't continue.
                    throw new RuleParserException("Bad format for input string." +
                                                  " Can't be a command before the chains");
                } else if(lastFoundTable == null) {
                    // No table was found. Can't continue.
                    throw new RuleParserException("Bad format for input string." +
                                                  " Can't be a command before the table");
                }

                // Here what we have is a command where tipically the last argument is a rule.
                // Tipical commands are for inserting rules, but who knows what we can find ...

                // Use the rule parser to build a GenericCommand from the line that
                // contains all the information in the line

                GenericCommand gCmd = cmdParser.ParseCommand(line, lastFoundTable);

                if(gCmd != null && gCmd.Rule != null) {
                    tableSet.Exec(gCmd);
                } else {
                    throw new RuleParserException("Bad format for input string. Can't get the rule for: \n" + line);
                }
            } else {
                throw new RuleParserException("Can't parse line: " + line);
            }
        }

        /// <summary>
        /// Gets if the string is a valid table. If it is the enum that matches
        /// the table is set in the output parameter.
        /// </summary>
        protected bool TryGetTableType(string line, out PacketTableType table) {
            line = line.Trim();
            object obj = null;

            if(line.StartsWith("*")) {
                line = line.Substring(1).Trim();
            }

            if(!TypeUtil.TryGetEnumValue(typeof(PacketTableType), line, out obj)) {
                table = PacketTableType.Filter;
                return false;
            }

            table = (PacketTableType)obj;
            return true;
        }
    }
}
