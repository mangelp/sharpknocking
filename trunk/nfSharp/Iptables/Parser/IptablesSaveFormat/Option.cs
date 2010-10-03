// Option.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez <mangelp>at<gmail>dot<com>
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
using System.Text;
using System.Collections.Generic;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {
    /// <summary>
    /// Models one option in the command line
    /// </summary>
    /// <remarks>
    /// Each option can have multiple parameters after the parsing of the command line if that is allowed.
    /// How this option behaves is determined by the flags set to it
    /// </remarks>
    public class Option: IEquatable<Option> {
        //This flag controls if more aliases can be added
        private bool dontAddMoreAliases;

        /// <summary>
        /// Gets if more aliases can be added to this option using the AddAliases method.
        /// </summary>
        public bool CanAddMoreAliases {
            get {
                return !this.dontAddMoreAliases;
            }
        }

        private OptionFlags flags;

        /// <summary>
        /// Option flags that changes the behaviour of the parsing
        /// </summary>
        public OptionFlags Flags {
            get {
                return this.flags;
            } set {
                this.flags = value;
            }
        }

        private TypeAssertionFlags assertFlags;

        /// <summary>
        /// Type assertion flags. Defaults to string (no assertions)
        /// </summary>
        public TypeAssertionFlags AssertFlags {
            get {
                return this.assertFlags;
            } set {
                this.assertFlags = value;
            }
        }

        private string description;

        /// <summary>
        /// Description of the meaning of this option. Used to autogenerate help messages
        /// </summary>
        public string Description {
            get {
                return this.description;
            } set {
                this.description = value;
            }
        }

        private OptionCaller caller;

        /// <summary>
        /// Gets the instance of the helper used to call methods
        /// </summary>
        public OptionCaller Caller {
            get {
                return this.caller;
            }
        }

        private List<string> names;

        /// <summary>
        /// Gets an array with all the aliases for this option
        /// </summary>
        public string[] Aliases {
            get {
                return this.names.ToArray();
            }
        }

        private List<SimpleParameter> parameters;

        /// <summary>
        /// Parameter found in the command line that matches this option.
        /// This is the last parameter found if there was more than one.
        /// </summary>
        public SimpleParameter[] Parameters {
            get {
                return parameters.ToArray();
            }
        }

        /// <summary>
        /// Gets the number of paramter that matched any alias of this option
        /// </summary>
        public int ParamCount {
            get {
                return parameters.Count;
            }
        }

        /// <summary>
        /// Returns an array with all the values of the parameter of this option
        /// </summary>
        internal string[] Values {
            get {
                string[] values = new string[this.parameters.Count];
                for(int i=0; i < this.parameters.Count; i++) {
                    values[i] = this.parameters[i].Value;
                }

                if (values.Length == 0 && this.hasDefaultValue)
                    return new string[] {this.defaultValue};

                return values;
            }
        }

        private string defaultValue;

        /// <summary>
        /// Default value for the option
        /// </summary>
        /// <remarks>
        /// This value must be used if the value to the parameters were optional.
        /// A null value here means that no value was set to this property.
        /// </remarks>
        public string DefaultValue {
            get {
                return this.defaultValue;
            } set {
                this.defaultValue = value;
                this.hasDefaultValue = value != null;
            }
        }

        private bool hasDefaultValue;

        /// <summary>
        /// Gets if the option has a default value
        /// </summary>
        public bool HasDefaultValue {
            get {
                return this.hasDefaultValue;
            }
        }

        private int group;

        /// <summary>
        /// Group for this option
        /// </summary>
        public int Group {
            get {
                return this.group;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Option()
        :this(null) {

        }

        /// <summary>
        /// Constructor. Initiallizes the owner of this option
        /// </summary>
        /// <param name="owner">
        /// A <see cref="OptionParser"/>
        /// </param>
        public Option(object owner) {
            this.names = new List<string>();
            this.caller = new OptionCaller(owner);
            this.flags = OptionFlags.Defaults;
            this.parameters = new List<SimpleParameter>();
        }

        //Ordered insertion into the array of aliases
        /// <summary>
        /// We insert the alias in the array keeping the order.
        /// </summary>
        /// <summary>
        /// This will help us when getting a hash code equal for
        /// all options as two options with the same aliases will
        /// have them in the same order.
        /// </summary>
        /// <param name="name">
        /// A <see cref="System.String"/>
        /// </param>
        private void AddAlias(string name) {
            int pos = this.names.BinarySearch(name);
            if (pos < 0) {
                pos = ~pos;
                if (pos < names.Count) {
                    this.names.Insert(pos, name);
                } else {
                    this.names.Add(name);
                }
            }
        }

        /// <summary>
        /// Adds aliases for the option's name. This method returns the reference to the instace.
        /// </summary>
        /// <param name="names">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>The reference to the instance where this method is executed</returns>
        public Option AddAliases(params string[] names) {
            foreach(string alias in names) {
                if (this.names.Contains(alias)) {
                    throw new ArgumentException("The name "+alias+" is already assigned.");
                }
                this.AddAlias(alias);
            }

            return this;
        }

        /// <summary>
        /// Disables the adding of new aliases to this option using the AddAlias method
        /// </summary>
        /// <remarks>This prevents the user to mess with aliases once they are set</remarks>
        public void DisableAliasAdding() {
            this.dontAddMoreAliases = true;
        }

        /// <summary>
        /// Gets if the option has all the aliases in an array
        /// </summary>
        /// <param name="names">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// <remarks>
        /// This operation does a binary search over the alias array so its O(log(n))
        /// but the size of the alias array is usually between 1 and 5 so this has no
        /// benefit in the real world :p
        /// </remarks>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool IsAlias(params string[] names) {
            foreach(string alias in names) {
                if (this.names.BinarySearch(alias) < 0) {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Gets if the option has the same aliases that other option
        /// </summary>
        /// <param name="opt">
        /// A <see cref="Option"/>
        /// </param>
        /// <remarks>
        /// This operation internally uses IsAlias(string[])
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool IsAlias(Option opt) {
            return this.IsAlias(opt.names.ToArray());
        }

        /// <summary>
        /// Gets if the option has any of the aliases of other option
        /// </summary>
        /// <param name="names">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool IsAliasAny(params string[] names) {
            foreach(string alias in names) {
                if (this.names.Contains(alias)) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets if the option has any of the aliases of other option
        /// </summary>
        /// <param name="opt">
        /// A <see cref="Option"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool IsAliasAny(Option opt) {
            return this.IsAliasAny(opt.names.ToArray());
        }

        /// <summary>
        /// Adds a parameter to the option.
        /// This method returns an instance of the object.
        /// </summary>
        /// <param name="param">
        /// A <see cref="SimpleParameter"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddParameter(SimpleParameter param) {
            this.parameters.Add(param);
            return this;
        }

        /// <summary>
        /// Sets flags to the option. This method returns an instance of the object.
        /// This method returns an instance of the object.
        /// </summary>
        /// <param name="flag">
        /// A <see cref="OptionFlags"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddFlag(OptionFlags flag) {
            this.flags |= flag;
            return this;
        }

        /// <summary>
        /// Gets if the option has a certain flag combination set. This method returns an instance of the
        /// object.
        /// </summary>
        /// <param name="flag">
        /// A <see cref="OptionFlags"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool HasFlag(OptionFlags flag) {
            return (this.flags & flag) == flag;
        }

        /// <summary>
        /// Sets the default value. This method returns an instance of the object.
        /// </summary>
        /// <param name="obj">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option SetDefaultValue(string obj) {
            this.defaultValue = obj;
            this.hasDefaultValue = (obj != null);
            return this;
        }

        /// <summary>
        /// Sets the group. This method returns an instande of the object.
        /// </summary>
        /// <param name="g">
        /// A <see cref="System.Int32"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option SetGroup(int g) {
            this.group = g;
            return this;
        }

        /// <summary>
        /// Removes all parameters set to this option.
        /// </summary>
        public void ClearState() {
            this.parameters.Clear();
        }

        /// <summary>
        /// Sets the assertion flags and the default value. This replaces previous values.
        /// </summary>
        /// <param name="flag">
        /// A <see cref="TypeAssertionFlags"/>
        /// </param>
        /// <param name="defaultValue">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option SetAssert(TypeAssertionFlags flag, string defaultValue) {
            this.defaultValue = defaultValue;
            this.assertFlags = flag;
            return this;
        }

        /// <summary>
        /// Adds assertion flags that will be added to existing flags.
        /// </summary>
        /// <param name="flag">
        /// A <see cref="TypeAssertionFlags"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option AddAssertFlag(TypeAssertionFlags flag) {
            this.assertFlags |= flag;
            return this;
        }

        /// <summary>
        /// Gets if the assertion flags is set with the value specified
        /// </summary>
        /// <param name="flag">
        /// A <see cref="TypeAssertionFlags"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool HasAssertFlag(TypeAssertionFlags flag) {
            return (this.assertFlags & flag) == flag;
        }

        /// <summary>
        /// Asserts that the values of the parameters and the default value follows the indication
        /// of the assertion flags.
        /// </summary>
        public void AssertValues() {
            throw new NotImplementedException("Not implemented. Developer run out of free time :(");
        }

        /// <summary>
        /// Sets the method name that the caller can use to invoke a method when the
        /// option is used.
        /// </summary>
        /// <param name="methodName">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="owner">Instance/type of the object that has defined the method</param>
        /// <remarks>
        ///
        /// If the owner is an instance the method is assumed to be an instance method, but if the
        /// owner is a type the method is assumed to be an static one.
        /// The owner can be set also throught the constructor.
        /// </remarks>
        /// <returns>
        /// The <see cref="Option"/> instance that can be used to call another method
        /// </returns>
        public Option SetCallerMethodName(string methodName, object owner) {
            this.caller.MethodName = methodName;
            if (owner != null) {
                this.caller.Owner = owner;
            }
            return this;
        }

        /// <summary>
        /// Sets the method delegate that will be invoked when this option is used
        /// in the command line
        /// </summary>
        /// <param name="deleg">
        /// A <see cref="OptionActionDelegate"/>
        /// </param>
        public Option SetCallerMethodDelegate(OptionActionDelegate deleg) {
            this.caller.ProcesingDelegate = deleg;
            return this;
        }

        /// <summary>
        /// Sets a description about the meaning of the option that can be used to generate
        /// help tips.
        /// </summary>
        /// <param name="desc">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="Option"/>
        /// </returns>
        public Option SetDescription(string desc) {
            this.description = desc;
            return this;
        }

        /// <summary>
        /// Returns the value of the first parameter related to this option if any or the
        /// default value of this option is was defined.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> with the value or null if no value was found
        /// </returns>
        public string GetValue() {
            if (this.parameters.Count > 0) {
                return this.parameters[0].Value;
            }
            return this.defaultValue;
        }

        /// <summary>
        /// Returns an array with all the values in all the parameters related to this option
        /// found in the command line.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> array with all the values or the default value if no
        /// parameter was found and a default value was set or an emtpy array if not.
        /// </returns>
        public string[] GetValues() {
            return this.Values;
        }

        /// <summary>
        /// Compares two options
        /// </summary>
        /// <param name="opt">
        /// A <see cref="Option"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        public bool Equals(Option opt) {
            if (this.Aliases.Length != opt.Aliases.Length
                    || !this.IsAlias(opt.Aliases)
                    || this.AssertFlags != opt.AssertFlags
                    || this.DefaultValue != opt.DefaultValue
                    || this.Flags != opt.Flags) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Compares two objects
        /// </summary>
        /// <param name="obj">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.Boolean"/>
        /// </returns>
        override public bool Equals(object obj) {
            if (!(obj is Option)) {
                return false;
            }

            return this.Equals((Option)obj);
        }

        /// <summary>
        /// Gets the hash code for this instance
        /// </summary>
        /// <returns>
        /// A <see cref="System.Int32"/>
        /// </returns>
        override public int GetHashCode() {
            string key = String.Empty;
            foreach(string str in this.Aliases) {
                key += str;
            }

            key+=this.AssertFlags;
            key+=this.DefaultValue;
            key+=this.Flags;

            return key.GetHashCode();
        }

        /// <summary>
        /// Gets an string that represents this instance
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString() {
            return this.ToString(String.Empty);
        }

        /// <summary>
        /// Gets an string that represents this instance
        /// </summary>
        /// <param name="value">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public string ToString(string value) {
            string defName = this.names[0];

            if (defName.Length > 1) {
                defName = "--"+defName;
            } else {
                defName = "-"+defName;
            }

            if (!String.IsNullOrEmpty(value)) {
                if (!this.HasFlag(OptionFlags.ValueRequired)) {
                    throw new InvalidOperationException("This option doesn't have a value");
                }
                defName += " " + value;
            } else if (this.HasDefaultValue && !String.IsNullOrEmpty(this.DefaultValue+String.Empty)) {
                defName += " " + this.DefaultValue;
            }

            return defName;
        }
    }
}
