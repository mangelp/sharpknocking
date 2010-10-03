// OptionParser.cs
//
//  Copyright (C) 2008 Miguel Angel Perez <email://mangelp_at/gmail?dot=com
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
using System.Collections;
using System.Collections.Generic;

using CommonUtilities.Types;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {
    /// <summary>
    /// Models a parser for options.
    /// </summary>
    public class OptionParser {
        /// <summary>
        /// Notifies when an option is found
        /// </summary>
        public event EventHandler<OptionCallerData> OptionFound;

        /// <summary>
        /// Notifies people about an error in the parameters
        /// </summary>
        public event EventHandler<OptionCallerData> ErrorFound;

        private object owner;

        /// <summary>
        /// Default owner for the methods to execute
        /// </summary>
        public object Owner {
            get {
                return this.owner;
            }
        }

        private bool throwExOnError;

        /// <summary>
        /// Gets/Sets a flag that causes an exception to be thrown when the parser founds an error
        /// </summary>
        public bool ThrowExOnError {
            get {
                return throwExOnError;
            } set {
                this.throwExOnError = value;
            }
        }

        private bool showMessage;

        /// <summary>
        /// Gets/Sets a flag that causes a message to be printed to console output when the parser
        /// founds an error.
        /// </summary>
        public bool ShowMessage {
            get {
                return this.showMessage;
            } set {
                this.showMessage = value;
            }
        }

        private List<Option> options;

        /// <summary>
        /// Gets an array with all the options that this parser have defined.
        /// </summary>
        public Option[] Options {
            get {
                return this.options.ToArray();
            }
        }

        /// <summary>
        /// Gets an option from its name
        /// </summary>
        /// <returns>
        /// An option if the name matches any alias of an option. Null otherwise.
        /// </returns>
        public Option this[string optName] {
            get {
                return this.Find(optName);
            }
        }

        private List<SimpleParameter> parameters;

        /// <summary>
        /// Gets an array with all the parameters of this option
        /// </summary>
        public SimpleParameter[] Parameters {
            get {
                return parameters.ToArray();
            }
        }

        private Option defaultOption;

        /// <summary>
        /// Default option of the parser
        /// </summary>
        public Option DefaultOption {
            get {
                return this.defaultOption;
            }
        }

        /// <summary>
        /// Constructor. Initializes the object without a default owner
        /// </summary>
        public OptionParser()
        :this(null) {

        }

        /// <summary>
        /// Constructor. Initializes the default owner
        /// </summary>
        public OptionParser(object owner) {
            this.parameters = new List<SimpleParameter>();
            this.options = new List<Option>();
            this.owner = owner;
        }

        /// <summary>
        /// Finds an option with the name in his aliases
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option Find(string name) {
            foreach(Option opt in this.options) {
                if (opt.IsAliasAny(name)) {
                    return opt;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets if the option contains a parameter with the name in his aliases
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool Contains(string name) {
            return this.Find(name) != null;
        }

        /// <summary>
        /// Removes all the parameter data
        /// </summary>
        public void ClearState() {
            this.parameters.Clear();
            foreach(Option opt in this.options) {
                opt.ClearState();
            }
        }

        /// <summary>
        /// Extracts the parameter list from a string.
        /// </summary>
        /// <remarks>
        /// Simply returns an array of objects where two properties are set,
        /// the parameter name and the parameter value. This is helpful when
        /// trying to analize the input.
        /// </remarks>
        public void Parse(string line) {
            //Console.WriteLine("Get parameter list'1");
            //Convert the line to a list of strings and pass it to the overload
            //of this method that does the work
            Parse(StringUtil.BreakCommandLine(ref line).ToArray());
        }

        /// <summary>
        /// Gets an array of parameters from an array of strings
        /// </summary>
        /// <param name="args">
        /// A <see cref="System.String"/>
        /// </param>
        /// <remarks>
        /// Simply returns an array of objects where two properties are set,
        /// the parameter name and the parameter value. This is helpful when
        /// trying to analize the input.
        /// </remarks>
        /// <returns>
        /// A <see cref="SimpleParameter"/> array with all the parameters found
        /// in the command line even if there are no options set to match them
        /// </returns>
        public void Parse(string[] args) {
            //First we need to ensure that there are no parameters left from previous
            //executions
            this.ClearState();

            int pos = 0;
            bool negateNext = false;
            SimpleParameter par = null;

            //Console.WriteLine("Procesing "+args.Length+" parameters");

            while (pos < args.Length) {
                if (String.IsNullOrEmpty(args[pos])) {
                    throw new OptionParserException("Found an empty string");
                } else if (args[pos].StartsWith("-")) {
                    par = new SimpleParameter();
                    par.Value = "";
                    bool longFormat = SimpleParameter.CheckLongFormat(args[pos]);
                    //Check the format of the options and get the name of the option
                    par.Name = (longFormat) ? args[pos].Substring(2) : args[pos].Substring(1);

                    if (String.IsNullOrEmpty(par.Name)) {
                        throw new OptionParserException("The parameter "+pos+" has an empty name");
                    }

                    //Check if the next parameter is a negated one
                    if (negateNext) {
                        //Console.WriteLine("Negating "+par.Name);
                        par.Not = true;
                        negateNext = false;
                    }
                    //Console.WriteLine("Adding parameter "+par);
                    this.parameters.Add(par);
                } else if (args[pos].StartsWith("!") && args[pos].Length == 1) {
                    //Console.WriteLine("Next have to be negated");
                    negateNext = true;
                    //Fix: if the negation if after the parameter is still valid behaviour as it
                    //negates the value
                    //} else if (negateNext) {
                    //throw new OptionParserException("Invalid '!' found before a value and after "+par.Name+" parameter.");
                } else if (par == null) {
                    throw new OptionParserException("Bad format. Found value without option");
                } else {
                    //Console.WriteLine("Adding value "+args[pos]+" to "+par.Name);
                    par.Value += String.IsNullOrEmpty(par.Value) ? args[pos] : " " + args[pos];
                }
                //Increment the counter
                pos++;
            }
        }

        /// <summary>
        /// Process all the parameters found and matches them with options
        /// </summary>
        /// <remarks>
        /// When a parameter doesn't match with any option alias this method throws
        /// OptionParserException with information about the parameter.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool ProcessOptions() {
            if (this.parameters.Count == 0) {
                this.UseDefaultOption();
                return false;
            }

            OptionCallerData data = new OptionCallerData();

            for(int i=0; i<this.parameters.Count; i++) {
                data.Position = i + 1;
                data.Parameter = this.parameters[i];
                data.SourceOption = this.Find(data.Parameter.Name);
                //Fix: Set the parameter in the option or we will loose this information
                if (data.SourceOption != null) {
                    data.SourceOption.AddParameter(data.Parameter);
                }
                this.OnOptionFound(data);
                if (data.AbortParsing) {
                    return false;
                }
            }

            return this.CheckRequiredOptions();
        }

        /// <summary>
        /// Sends the notification about an option that matches with a parameter in the command line.
        /// </summary>
        /// <param name="data">
        /// A <see cref="OptionCallerData"/>
        /// </param>
        protected virtual void OnOptionFound (OptionCallerData data) {
            //People are notified with this handler so they can cancel the next step
            if (this.OptionFound != null) {
                this.OptionFound(this, data);
            }
            //If there is an option and the flags are valid call the method
            if (data.SourceOption != null && !data.AbortParsing && this.CheckFlags(data)) {
                data.SourceOption.Caller.CallMethod(data);
            }
        }

        /// <summary>
        /// Sends the notification to others about an error in the parsing
        /// </summary>
        /// <param name="data">
        /// A <see cref="OptionCallerData"/>
        /// </param>
        protected virtual void OnErrorFound (OptionCallerData data) {
            //First notify people so they can disable the error erasing data and
            //recovering the state
            if (this.ErrorFound != null) {
                this.ErrorFound(this, data);
            }

            if (data.HasError && this.showMessage) {
                Console.WriteLine(data.ErrorMessage);
            }
            if (data.HasError && this.throwExOnError) {
                throw new OptionParserException(data.ErrorMessage);
            }
        }

//		private void DumpOption(Option opt)
//		{
//			Console.WriteLine( "[DumpOption]  "+
//			    String.Format(
//			        "Params: {0}, Flags: {1:x2}", opt.ParamCount, (int)opt.Flags));
//			foreach(SimpleParameter par in opt.Parameters) {
//				Console.WriteLine("   * "+par);
//			}
//		}

        /// <summary>
        /// Checks the flags of an option along with the parameter to ensure that they are valid.
        /// </summary>
        /// <param name="data">
        /// A <see cref="OptionCallerData"/>
        /// </param>
        protected virtual bool CheckFlags(OptionCallerData data) {
            OptionFlags flags = data.SourceOption.Flags;
            data.AbortParsing = true;
            //this.DumpOption(data.SourceOption);

            if ((flags & OptionFlags.ExistingPath) == OptionFlags.ExistingPath
                    && !File.Exists(data.Parameter.Value)
                    && !Directory.Exists(data.Parameter.Value)) {
                data.ErrorMessage = "Invalid path "+data.Parameter.Value + " in option with alias " +
                                    data.Parameter.Name;
            } else if ((flags & OptionFlags.Negable) != OptionFlags.Negable
                       && data.Parameter.Not) {
                data.ErrorMessage = "Option with alias " + data.Parameter.Name +
                                    " cannot use negation mark '!'";
            } else if ((flags & OptionFlags.Multiple) != OptionFlags.Multiple
                       && data.SourceOption.ParamCount > 1) {
                data.ErrorMessage = "Option with alias " + data.Parameter.Name +
                                    " is not multiple but has " + data.SourceOption.ParamCount + " parameters";
            } else if ((flags & OptionFlags.ValueRequired) == OptionFlags.ValueRequired
                       && String.IsNullOrEmpty(data.Parameter.Value)) {
                data.ErrorMessage = "Option with alias "+data.Parameter.Name+" requires a value";
            } else {
                data.AbortParsing = false;
            }

            if (data.HasError) {
                this.OnErrorFound(data);
            }

            return !data.AbortParsing;
        }

        private void UseDefaultOption() {
            if (this.defaultOption == null) {
                return;
            }
            OptionCallerData data = new OptionCallerData();
            data.SourceOption = this.defaultOption;
            data.Position = -1;
            this.OnOptionFound(data);
        }

        //Adds an option to the current list
        private Option InternalAddOption(object owner,
                                         string handlerMethod,
                                         OptionActionDelegate del,
                                         OptionFlags flags,
                                         int group,
                                         params string[] aliases) {
            if (owner == null) {
                owner = this.owner;
            }
            Option opt = new Option(owner);
            opt.Flags = flags;
            opt.AddAliases(aliases);
            opt.Caller.MethodName = handlerMethod;
            opt.Caller.ProcesingDelegate = del;
            opt.SetGroup(group);
            //Don't allow others to add more aliases
            opt.DisableAliasAdding();
            CheckOption(opt);
            this.options.Add(opt);
            return opt;
        }

        /// <summary>
        /// Checks the options that are required or those required but in a by-group basis
        /// </summary>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        private bool CheckRequiredOptions() {
            Dictionary<int, bool> groups = new Dictionary<int, bool>();
            OptionCallerData data = new OptionCallerData();

            foreach(Option opt in this.options) {

                if ((opt.Flags & OptionFlags.RequiredAny) == OptionFlags.RequiredAny) {
                    if (!groups.ContainsKey(opt.Group)) {
                        groups.Add(opt.Group, false);
                    }
                    if (opt.ParamCount > 0) {
                        groups[opt.Group] = true;
                    }
                }

                if ((opt.Flags & OptionFlags.Required) == OptionFlags.Required
                        && opt.ParamCount == 0) {
                    data.Parameter = null;
                    data.SourceOption = opt;
                    data.ErrorMessage = "The option with alias " + opt.Aliases[0] +
                                        " is required but was not found";
                    data.AbortParsing = true;
                    this.OnErrorFound(data);
                    if (data.AbortParsing) {
                        break;
                    }
                }
            }

            if (data.AbortParsing) {
                return false;
            }

            foreach(int key in groups.Keys) {
                if (groups[key]) {
                    continue;
                }
                data.Parameter = null;
                data.SourceOption = null;
                data.AbortParsing = true;
                data.ErrorMessage = "There is no options specified for group " + key + ".  ";
                data.ErrorMessage += "Options defined in that group: ";
                foreach(Option opt in this.options) {
                    data.ErrorMessage += opt.Aliases[0] + ", ";
                }
                this.OnErrorFound(data);
            }

            return !data.AbortParsing;
        }

        private void CheckOption(Option opt) {
            foreach(Option optOrig in this.options) {
                if (optOrig.IsAliasAny(opt)) {
                    throw new ArgumentException("Duplicated alias name found");
                }
            }

            if ((opt.Flags & OptionFlags.DefaultOption) == OptionFlags.DefaultOption) {
                if (this.defaultOption != null) {
                    throw new OptionParserException(
                        "Two options are marked to be the default. Only can be one");
                }
                this.defaultOption = opt;
            }

            if ((opt.Flags & OptionFlags.RequiredAny) == OptionFlags.RequiredAny
                    && opt.Group == 0) {
                throw new OptionParserException(
                    "The options required by group must have a valid group id greater than 0");
            }
        }

        /// <summary>
        /// Adds an option. This method returns the option created.
        /// </summary>
        /// <param name="handlerMethod">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="flags">
        /// A <see cref="OptionFlags"/>
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddOption(string handlerMethod, OptionFlags flags, params string[] aliases) {
            return this.InternalAddOption(null, handlerMethod, null, flags, 0, aliases);
        }

        /// <summary>
        /// Adds an option. This method returns the option created.
        /// </summary>
        /// <param name="del">
        /// A <see cref="OptionActionDelegate"/>
        /// </param>
        /// <param name="flags">
        /// A <see cref="OptionFlags"/>
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddOption(OptionActionDelegate del, OptionFlags flags, params string[] aliases) {
            return this.InternalAddOption(null, null, del, flags, 0, aliases);
        }

        /// <summary>
        /// Adds an option with the names specified as aliases
        /// </summary>
        /// <param name="aliases">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddOption(params string[] aliases) {
            return this.InternalAddOption(null, null, null, OptionFlags.Defaults, 0, aliases);
        }

        /// <summary>
        /// Adds an option. This method returns the instance of the parser
        /// </summary>
        /// <param name="opt">
        /// A <see cref="Option"/>
        /// </param>
        /// <returns>
        /// A <see cref="OptionParser"/>
        /// </returns>
        public OptionParser AddOption(Option opt) {
            this.options.Add(opt);
            return this;
        }

        /// <summary>
        /// Gets an array with the description of each option in a format usefull to use as command-line
        /// help text.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public string[] GetOptionsDescription() {
            Dictionary<string,string> msgs = new Dictionary<string,string>(this.options.Count);
            List<string> parts;
            string[] result = new string[this.options.Count];
            string optmsg;
            int max = 0;

            foreach(Option opt in this.options) {
                parts = new List<string>(opt.Aliases.Length);
                foreach(string alias in opt.Aliases) {
                    if (alias.Length == 1) {
                        parts.Add("-"+alias);
                    } else {
                        parts.Add("--"+alias);
                    }
                }
                optmsg = String.Join("|", parts.ToArray());
                if (optmsg.Length > max) {
                    max = optmsg.Length;
                }
                msgs.Add(optmsg, opt.Description);
            }

            int pos = 0;
            foreach(string key in msgs.Keys) {
                result[pos++] = key.PadRight(max+1) + ": " + msgs[key];
            }

            return result;
        }
    }
}
