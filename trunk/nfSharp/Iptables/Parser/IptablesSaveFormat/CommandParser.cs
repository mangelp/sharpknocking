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
using CommonUtilities.Types;
using System.Collections;
using NFSharp.Iptables.Core.Commands.Options;
using NFSharp.Iptables.Core.Commands;
using NFSharp.Iptables.Core.Extensions.Match;
using NFSharp.Iptables.Core.Extensions.Target;
using NFSharp.Iptables.Core;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {


    public class CommandParser {

        //Cache for the names and the enum type for each name
        private static Hashtable optNameCache;

        /// <summary>
        /// Static initialization of cached values
        /// </summary>
        static CommandParser() {
            //We are going to keep in memory the list of option names
            //as the keys of the hashtable and the enum constant value
            //as the value. This will speed up the search time and will cost
            //little memory
            optNameCache = new Hashtable();

            Array arr = Enum.GetValues(typeof(RuleCommands));
            string[] aliases;

            foreach(object obj in arr) {
                aliases = AliasUtil.GetAliases(obj);

                for(int i=0; i<aliases.Length; i++) {
                    optNameCache.Add(aliases[i], obj);
                }
            }
        }

        public CommandParser () {
        }

        /// <summary>
        /// Gets if the line can be a rule candidate.
        /// </summary>
        /// <remarks>
        /// This only checks if the line starts with a -. So this check is poor
        /// and only useful when reading lines in iptables config format (the
        /// same format as iptables-save output)
        /// </remarks>
        public bool CanBeACommand(string line) {
            return line.StartsWith("-");
        }

        /// <summary>
        /// Gets if the option name (parameter name without '-') matches one
        /// of the names (including aliases) of the commands.
        /// </summary>
        /// <remarks>
        /// We aren't going to implement the way iptables processes parameters
        /// in the command line (it can determine the name of the parameter if
        /// there are enough initial letters).
        /// The name must be specified as the short or long formats only (this
        /// includes aliases).
        /// </remarks>
        public bool IsCommand(string optName) {
            bool result = false;

            if(optNameCache.ContainsKey(optName)) {
                result = true;
            }

            //Debug.VerboseWrite("IsCommand("+optName+")"+optNameCache.Count+"? "+result, VerbosityLevels.High);
            return result;
        }

        /// <summary>
        /// Returns the Commands enumeration constant that matches the
        /// command name. The name can be any valid alias for the command but
        /// it can't be the name of a constant of the enumeration.
        /// </summary>
        /// <returns>
        /// Commands.None if the name is not a valid alias or the name is one
        /// of the enumeration constants.
        /// </returns>
        public RuleCommands GetCommandType(string cmd) {
            if(optNameCache.ContainsKey(cmd)) {
                return (RuleCommands)optNameCache[cmd];
            }

            return RuleCommands.None;
        }

        /// <summary>
        /// Builds a command from the string. This also parses every parameter
        /// like the rule.
        /// </summary>
        /// <param name="curTable">Table where is defined the chain that the
        /// command affects</param>
        /// <remarks>
        /// The line must be in the same format that iptables-save uses for output. So
        /// any line must start with a command followed by the parameters.
        /// In this context the real rules are parameters of the command so at the
        /// end the command will have in her properties everything.
        /// </remarks>
        public GenericCommand ParseCommand(string line, NetfilterTable curTable) {
            //Console.WriteLine("Processing line: "+line);
            //If the line doesn't look like a valid command we can't do nothing
            if(!this.CanBeACommand (line)) {
                return null;
            }

            SimpleParameter[] parameters = this.GetParameterList(line);

            if(parameters.Length == 0) {
                throw new ArgumentException("There are nothing to parse here: "+line);
            }

            if(!this.IsCommand(parameters[0].Name)) {
                //The first thing must be the command
                throw new FormatException("The line doesn't start with a command");
            }

            GenericCommand gCmd = null;
            Exception ex = null;

            // Try to get the command or throw an exception
            if(!IptablesCommandFactory.TryGetCommand(parameters[0], out gCmd, out ex)) {
                throw new FormatException("Error while parsing line: "+line, ex);
            }

            if(gCmd.MustSpecifyRule && parameters.Length < 2) {
                // With only 2 parameters there is no room for a rule specifcation.
                // Cry if it is required
                throw new FormatException("Unexpected parameters in line after command: "+line);
            }

            if(gCmd.MustSpecifyRule) {
                gCmd.Rule = new NetfilterRule();
            }

            GenericOption option=null;
            SimpleParameter currParam;
            int pos = 1;
            MatchExtensionHandler matchExHandler = null;
            TargetExtensionHandler targetExHandler = null;

            //Now we must parse the rest of the parameters and add them to the command
            while(pos <parameters.Length) {
                currParam = parameters[pos];
                //Console.WriteLine("Processing["+pos+"-"+(parameters.Length-1)+"]: "+currParam);
                //Give to the parameter the correct procesing based in the guess
                //of his type
                if(GenericOption.IsOption(currParam.Name)) {
                    //If this command doesn't have a rule there can't be more than the
                    //parameters for the command that where already grouped into the
                    //first SimpleParameter object. So if there is a parameter something
                    //got broken and we must give back an exception
                    if(!gCmd.MustSpecifyRule) {
                        throw new FormatException("Found rule where no rule was expected");
                    }
                    //We use a factory to build an option object
                    else if(!IptablesOptionFactory.TryGetOption(currParam, out option, out ex)) {
                        throw ex;
                    }
                    //Console.WriteLine("Adding option parameter: "+currParam);
                    //Add the option to the rule
                    gCmd.Rule.Options.Add(option);
                } else if(gCmd.Rule.TryGetMatchExtensionHandler(currParam.Name, out matchExHandler)) {
                    //Console.WriteLine("Adding a match extension parameter: "+currParam);
                    //The parameter is an option for a match extension. We add to it
                    //TODO: If the name is not a valid parameter we get a null reference exception
                    //check that.
                    matchExHandler.AddParameter(currParam.Name, currParam.Value);
                } else if(gCmd.Rule.TryGetTargetExtensionHandler(currParam.Name, out targetExHandler)) {
                    //Console.WriteLine("Adding a target extension parameter: "+currParam.Name+", "+currParam.Value);
                    //The parameter is an option or the target extension of the rule
                    //We add to it
                    targetExHandler.AddParameter(currParam.Name, currParam.Value);
                }

                pos++;
            }

            return gCmd;
        }


        /// <summary>
        /// Extracts the parameter list from the line.
        /// </summary>
        /// <remarks>
        /// The parameters are not procesed so this method doesn't know nothing
        /// about match extensions, targets or actions.<br/>
        /// Simply returns an array of objects where two properties are set,
        /// the parameter name and the parameter value. This is helpful when
        /// trying to analize the rule.
        /// </remarks>
        public SimpleParameter[] GetParameterList(string line) {
            string[] parts = StringUtil.Split(line, true, ' ');

            ArrayList temp = new ArrayList();
            int pos=0;
            bool negateNext=false;

            //Convert parameters and values into objects
            SimpleParameter par = null;

            while(pos<parts.Length) {
                //If the part starts with - is a parameter. If not it is something
                //unknowm
                if(parts[pos][0]=='-') {
                    par = new SimpleParameter();

                    if(negateNext) {
                        par.Not = true;
                        negateNext = false;
                    }

                    bool longFormat = SimpleParameter.CheckLongFormat(parts[pos]);

                    if(longFormat) {
                        par.Name = parts[pos].Substring(2);
                    } else {
                        par.Name = parts[pos].Substring(1);
                    }

                    pos++;
                    par.Value = String.Empty;

                    //Check for negation at the start of values for the current
                    //parameter

                    if(parts[pos].Length==1 && parts[pos][0]=='!') {
                        par.Not = true;
                        pos++;
                    }

                    //if the next thing is not a parameter is the value of this
                    //parameter.
                    while(pos<parts.Length && parts[pos][0]!='-') {
                        if(!String.IsNullOrEmpty(par.Value)) {
                            par.Value += " "+parts[pos];
                        } else {
                            par.Value = parts[pos];
                        }

                        //If there is a ! the next must be a parameter. If not
                        //there is a bad format.
                        if(parts[pos][0]=='!') {
                            if((pos+1)<parts.Length &&
                                    parts[pos+1][0]=='-') {
                                negateNext = true;
                            } else {
                                throw new NetfilterException("Invalid '!' found");
                            }
                        }

                        //Go to the next position
                        pos++;
                    }

                    //Debug.Write("GetParameterList: New parameter: "+par);

                    //Don't forget to add the object to the temporal list
                    temp.Add(par);

                    //skip the next increment to process the current parameter
                    continue;
                } else if(parts[pos][0]=='!') {
                    // We found negation prior to parameter. Is the negation of
                    // the next parameter.
                    negateNext = true;
                }

                pos++;
            }

            //Convert the arrayList to an array of the correct type
            SimpleParameter[] result = new SimpleParameter[temp.Count];

            for(int j=0; j<temp.Count; j++) {
                //Debug.Write("GetParameterList: Adding to result: "+temp[j]);
                result[j] = (SimpleParameter)temp[j];
            }

            return result;
        }


    }
}
