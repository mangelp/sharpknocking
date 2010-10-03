// CmdLineOptionsParser.cs
//
//  Copyright (C)  2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp_AT_gmail_DOT_com)
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

using System;
using System.Reflection;
using System.Collections.Generic;

using CommonUtilities.Types;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {
    /// <summary>
    /// Option parser for command lines
    /// </summary>
    public class CmdLineOptionsParser {
        private List<SimpleParameter> parameters;

        /// <summary>
        /// Gets an array with all the parameters found after parsing a
        /// commnand line
        /// </summary>
        public SimpleParameter[] Parameters {
            get {
                return this.parameters.ToArray();
            }
        }

        private Type methodsOwner;
        private object methodsOwnerInstance;
        private List<CmdLineOption> options;

        private CmdLineOption defaultOption;
        private string defaultOptionName;

        /// <summary>
        /// Default option to use when no other have been specified
        /// </summary>
        public string DefaultOptionName {
            get {
                return defaultOptionName;
            } set {
                this.defaultOptionName = value;
            }
        }

        private bool allowNoOptions;

        /// <summary>
        /// If true allows an empty set of options after the procesing. If not
        /// the parer will cry when no options are specified and recommend the
        /// default option if one was specified.
        /// </summary>
        public bool AllowNoOptions {
            get {
                return allowNoOptions;
            } set {
                this.allowNoOptions = value;
            }
        }

        /// <summary>
        /// Gets an option with the name if it exists
        /// </summary>
        public CmdLineOption this[string name] {
            get {
                return this.FindOption(name);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public CmdLineOptionsParser() {
            this.options = new List<CmdLineOption>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="methodsOwner">
        /// A <see cref="Type"/> of the object that holds the static methods to call
        /// </param>
        public CmdLineOptionsParser(Type methodsOwner) {
            this.options = new List<CmdLineOption>();
            this.allowNoOptions = true;
            this.defaultOptionName = String.Empty;
            this.methodsOwner = methodsOwner;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="methodsOwnerInstance">
        /// A <see cref="System.Object"/> reference to the instance of the object
        /// with the methods to execute.
        /// </param>
        public CmdLineOptionsParser(object methodsOwnerInstance) {
            this.options = new List<CmdLineOption>();
            this.methodsOwnerInstance = methodsOwnerInstance;
            //This and the delegate must be null to see that we want to use
            //the instance
            this.methodsOwner = null;
        }

        /// <summary>
        /// Extracts the parameter list from a string.
        /// </summary>
        /// <remarks>
        /// Simply returns an array of objects where two properties are set,
        /// the parameter name and the parameter value. This is helpful when
        /// trying to analize the input.
        /// </remarks>
        public void ProcessParameters(string line) {
            //Console.WriteLine("Get parameter list'1");
            //Convert the line to a list of strings and pass it to the overload
            //of this method that does the work
            ProcessParameters(StringUtil.BreakCommandLine(ref line).ToArray());
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
        public void ProcessParameters(string[] args) {
            this.SetDefaultOption();
            //Temporal list to add parameters as they are found
            this.parameters = new List<SimpleParameter>();
            int pos = 0;
            bool negateNext = false;
            SimpleParameter par = null;

            //Console.WriteLine("Procesing "+args.Length+" parameters");

            while (pos < args.Length) {

                //Console.WriteLine("Args["+pos+"]= "+args[pos]);
                //Console.WriteLine("Pos: "+pos);

                //If the part starts with - is a parameter. If not it is something
                //unknowm like the values of previous parameters

                if (String.IsNullOrEmpty(args[pos])) {
                    throw new OptionParserException("Found an empty string");
                } else if (args[pos].StartsWith("-")) {
                    par = new SimpleParameter();
                    par.Value = "";
                    bool longFormat = SimpleParameter.CheckLongFormat(args[pos]);
                    //Check the format of the options and get the name of the option
                    if (longFormat) {
                        par.Name = args[pos].Substring(2);
                    } else {
                        par.Name = args[pos].Substring(1);
                    }

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

            for (int i=0; i<this.parameters.Count; i++) {
                if (!this.ProcessParameter(i)) {
                    break;
                }
            }

            if (this.parameters.Count == 0 && this.allowNoOptions &&
                    this.defaultOption != null) {
                this.ProcessParameter(defaultOption);
            }
        }

        /// <summary>
        /// Sets an option as the default whose handler have to be invoked when no option have been specified.
        /// </summary>
        private void SetDefaultOption() {
            if (String.IsNullOrEmpty(this.defaultOptionName)) {
                return;
            }
            //Console.WriteLine("Looking for " + this.defaultOptionName);
            this.defaultOption = this.FindOption(this.defaultOptionName);
            if (this.defaultOption == null)
                throw new Exception("Can't set default option " + this.defaultOptionName
                                    + ". It doesn't exists");
        }

        /// <summary>
        /// Adds a new option to the parser to perform user-defined actions when
        /// a parameter that matches the option name is found.
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/> array with the aliases for the option
        /// </param>
        public void AddOption(string name, params string[] aliases) {
            this.InternalAddOption(name, String.Empty, null, aliases);
        }

        /// <summary>
        /// Adds a new option with a delegate that handles it when found
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> with the name of the option
        /// </param>
        /// <param name="pdel">
        /// A <see cref="OptionProcessingDelegate"/> that will handle the option
        /// when found
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/> array with the aliases for the option
        /// </param>
        public void AddOptionWithDelegate(string name, OptionProcessingDelegate pdel,
                                          params string[] aliases) {
            this.InternalAddOption(name, String.Empty, pdel, aliases);
        }

        /// <summary>
        /// Adds a option with a method name that handles it when found
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> with the name of the optioin
        /// </param>
        /// <param name="method">
        /// A <see cref="System.String"/> with the name of the method that handles
        /// the option when found
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/> array with the names of the aliases for
        /// the option
        /// </param>
        public void AddOptionWithMethod (string name, string method,
                                         params string[] aliases) {
            this.InternalAddOption(name, method, null, aliases);
        }

        /// <summary>
        /// Adds a new option and ways to execute a method to perform aditional
        /// processing when an option is found
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/> with the name of the option
        /// </param>
        /// <param name="methodName">
        /// A <see cref="System.String"/> with the name of the method that handles
        /// the option when found
        /// </param>
        /// <param name="pdel">
        /// A <see cref="OptionProcessingDelegate"/> to the method that handles the
        /// option when found
        /// </param>
        /// <param name="aliases">
        /// A <see cref="System.String"/> array of strings with the aliases for the
        /// command
        /// </param>
        private void InternalAddOption(string name, string methodName,
                                       OptionProcessingDelegate pdel, params string[] aliases) {
            if (String.IsNullOrEmpty(name)) {
                throw new ArgumentException("The option name can't be null or empty");
            }

            //Console.WriteLine(
            //      String.Format("Adding option Name:{0}, IsLong:{1}, MethodName:{2},
            //                    HasDelegate:{3}, Args:{4}",
            //                    name, isLong, methodName, pdel!=null,
            //                    aliases.Length));

            List<string> faliases = new List<string>(aliases);
            faliases.Add(name);
            CmdLineOption opt = this.FindOption(faliases.ToArray());
            if (opt != null) {
                throw new Exception("An option with the same alias already exists");
            }
            opt = new CmdLineOption(name, pdel);
            opt.MethodName = methodName;
            opt.SetAliases(aliases);
            this.InternalAddOption(opt);
        }

        //** Private stuff **//

        /// <summary>
        /// Calls a method using reflection
        /// </summary>
        /// <param name="data">
        /// A <see cref="OptionCallData"/>
        /// </param>
        private void CallMethod(OptionCallData data) {
            if (this.methodsOwner != null) {
                //If there is a methodOwner type specified use it to execute the method
                //that is required to be public and static
                //Console.WriteLine("Calling method: "+data.SourceOption.MethodName+"
                //                   from "+this.methodsOwner.FullName+" type");
                BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public |
                                     BindingFlags.Static;
                this.methodsOwner.InvokeMember(data.SourceOption.MethodName, flags,
                                               null, null, new object[] {data});
            } else if (this.methodsOwnerInstance != null) {
                //If there is a methodOwnerInstance specified use it to execute the
                //method that is required to be public
                //Console.WriteLine("Calling method: "+data.SourceOption.MethodName+
                //                  " from "+this.methodsOwnerInstance.GetType().FullName+" instance");
                BindingFlags flags = BindingFlags.InvokeMethod | BindingFlags.Public
                                     | BindingFlags.Instance;
                Type t = this.methodsOwnerInstance.GetType();
                t.InvokeMember(data.SourceOption.MethodName, flags, null,
                               this.methodsOwnerInstance, new object[] {data});
            } else {
                throw new InvalidOperationException (
                    "Can't call method "+data.SourceOption.MethodName
                    +". The source type or instance are not specified");
            }

        }

        /// <summary>
        /// Adds an option to the list using ordered insertion
        /// </summary>
        /// <param name="opt">
        /// A <see cref="CmdLineOption"/> to add
        /// </param>
        /// <remarks>
        /// This method uses binary search to look for the elements in the list
        /// as the list is kept ordered by insertion so search over the list
        /// has a good O(log[n])
        /// </remarks>
        private void InternalAddOption(CmdLineOption opt) {
            //Console.WriteLine("<Internal>: Adding option: "+opt.Name);
            int pos = this.options.BinarySearch(opt);
            if(pos >= 0) {
                throw new ArgumentException("The option "+opt.Name+" is already added");
            }
            pos = ~pos;
            if(pos<this.options.Count) {
                this.options.Insert(pos, opt);
            } else {
                this.options.Add(opt);
            }
        }

        /// <summary>
        /// Looks for an option with the desired name as an alias
        /// </summary>
        /// <param name="aliases">
        /// A <see cref="System.String"/> to search for
        /// </param>
        /// <returns>
        /// A <see cref="CmdLineOption"/> that has that name as an alias or null if not
        /// </returns>
        public CmdLineOption FindOption(params string[] aliases) {
            if (aliases == null || aliases.Length == 0) {
                return null;
            }
            foreach(CmdLineOption opt in this.options) {
                if (opt.IsAlias(aliases)) {
                    return opt;
                }
            }
            return null;
        }

        private bool ProcessParameter(CmdLineOption opt) {
            OptionCallData data = new OptionCallData();
            data.SourceOption = opt;
            if (data.SourceOption.ProcesingDelegate != null) {
                data.SourceOption.ProcesingDelegate(data);
            } else if (!String.IsNullOrEmpty(data.SourceOption.MethodName)) {
                this.CallMethod(data);
            }
            return !data.AbortParsing;
        }

        /// <summary>
        /// Processes only one parameter
        /// </summary>
        /// <param name="order">
        /// A <see cref="System.Int32"/> with the position of the parameter in the
        /// list.
        /// </param>
        private bool ProcessParameter(int order) {
            SimpleParameter par = this.parameters[order];
            //Console.WriteLine("ProcessingParameter: "+par);
            //first search the option

            //Console.WriteLine("Procesing option "+this.options[pos].Name);
            //Then set the parameter and call the handler if it exists
            OptionCallData data = new OptionCallData();
            data.SourceOption = this.FindOption(par.Name);
            if (data.SourceOption == null) {
                throw new Exception("Option not found for parameter " + par.Name);
            }
            //Set the parameter in the option to update counters and make available
            data.SourceOption.SetParameter(par, order);
            //Console.WriteLine("Procesing option");
            if (data.SourceOption.ProcesingDelegate != null) {
                data.SourceOption.ProcesingDelegate(data);
            } else if (!String.IsNullOrEmpty(data.SourceOption.MethodName)) {
                this.CallMethod(data);
            }

//			if(data.AbortParsing)
//				Console.WriteLine(data.ErrorMessage);

            return !data.AbortParsing;
        }

    }
}
