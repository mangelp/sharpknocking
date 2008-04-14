// Option.cs
//
//  Copyright (C) 2008 [name of author]
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

namespace Developer.Common.Options
{
	/// <summary>
	/// Models one option in the command line
	/// </summary>
	/// <remarks>
	/// Each option can have multiple parameters after the parsing of the command line if that is allowed.
	/// How this option behaves is determined by the flags set to it
	/// </remarks>
	public class Option
	{
		//This flag controls if more aliases can be added
		private bool dontAddMoreAliases;
		
		/// <summary>
		/// Gets if more aliases can be added to this option using the AddAliases method.
		/// </summary>
		public bool CanAddMoreAliases
		{
			get {
				return !this.dontAddMoreAliases;
			}
		}
		
		private OptionFlags flags;
		
		/// <summary>
		/// Option flags that changes the behaviour of the parsing
		/// </summary>
		public OptionFlags Flags
		{
			get {
				return this.flags;
			}
			set {
				this.flags = value;
			}
		}
		
		private TypeAssertionFlags assertFlags;
		
		/// <summary>
		/// Type assertion flags. Defaults to string (no assertions)
		/// </summary>
		public TypeAssertionFlags AssertFlags
		{
			get {
				return this.assertFlags;
			}
			set {
				this.assertFlags = value;
			}
		}
		
		private string description;
		
		/// <summary>
		/// Description of the meaning of this option. Used to autogenerate help messages
		/// </summary>
		public string Description
		{
			get {
				return this.description;
			}
			set {
				this.description = value;
			}
		}
		
		private OptionCaller caller;
		
		/// <summary>
		/// Gets the instance of the helper used to call methods
		/// </summary>
		public OptionCaller Caller
		{
			get {
				return this.caller;
			}
		}
		
		private List<string> names;
		
		/// <summary>
		/// Gets an array with all the aliases for this option
		/// </summary>
		public string[] Aliases
		{
			get {
				return this.names.ToArray();
			}
		}
		
		private List<SimpleParameter> parameters;
		
		/// <summary>
		/// Parameter found in the command line that matches this option.
		/// This is the last parameter found if there was more than one.
		/// </summary>
		public SimpleParameter[] Parameters
		{
			get { return parameters.ToArray();}
		}
		
		/// <summary>
		/// Gets the number of paramter that matched any alias of this option
		/// </summary>
		public int ParamCount
		{
			get {
				return parameters.Count;
			}
		}
		
		/// <summary>
		/// Returns an array with all the values of the parameter of this option
		/// </summary>
		internal string[] Values
		{
			get {
				string[] values = new string[this.parameters.Count];
				for(int i=0; i < this.parameters.Count; i++) {
					values[i] = this.parameters[i].Value;
				}
				
				if (values.Length == 0 && this.defaultValue != null)
					return new string[]{this.defaultValue};
				
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
		public string DefaultValue
		{
			get {
				return this.defaultValue;
			}
			set {
				this.defaultValue = value;
			}
		}
		
		private int group;
		
		/// <summary>
		/// Group for this option
		/// </summary>
		public int Group
		{
			get {
				return this.group;
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="owner">
		/// A <see cref="OptionParser"/>
		/// </param>
		public Option(object owner)
		{
			this.names = new List<string>();
			this.caller = new OptionCaller(owner);
			this.flags = OptionFlags.Defaults;
			this.parameters = new List<SimpleParameter>();
		}
		
		//Ordered insertion into the array of aliases
		private void AddAlias(string name)
		{
			int pos = this.names.BinarySearch(name);
			if (pos < 0) {
				pos = ~pos;
				if (pos < names.Count)
					this.names.Insert(pos, name);
				else
					this.names.Add(name);
			}
		}
		
		/// <summary>
		/// Adds aliases to this option not erasing previous aliases.
		/// </summary>
		/// <param name="names">
		/// A <see cref="System.String"/>
		/// </param>
		public void AddAliases(params string[] names)
		{
			foreach(string alias in names) {
				if (this.names.Contains(alias))
					throw new ArgumentException("The name "+alias+" is already assigned.");
				this.AddAlias(alias);
			}
		}
		
		/// <summary>
		/// Disables the adding of new aliases to this option using the AddAlias method
		/// </summary>
		public void DisableAliasAdding()
		{
			this.dontAddMoreAliases = true;
		}
		
		/// <summary>
		/// Gets if the option has the same aliass that other option
		/// </summary>
		/// <param name="names">
		/// A <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsAlias(params string[] names)
		{
			foreach(string alias in names) {
				if (!this.names.Contains(alias))
					return false;
			}
			return true;
		}
		
		/// <summary>
		/// Gets if the option has the same aliases that other option
		/// </summary>
		/// <param name="opt">
		/// A <see cref="Option"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool IsAlias(Option opt)
		{
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
		public bool IsAliasAny(params string[] names)
		{
			foreach(string alias in names) {
				if (this.names.Contains(alias))
					return true;
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
		public bool IsAliasAny(Option opt)
		{
			return this.IsAliasAny(opt.names.ToArray());
		}
		
		/// <summary>
		/// Compares two objects.
		/// </summary>
		/// <param name="obj">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public override bool Equals(object obj)
		{
			if (!(obj is Option))
				return false;
			return this.IsAlias((Option)obj);
		}
		
		/// <summary>
		/// Gets the hash code
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
		public override int GetHashCode()
		{
			StringBuilder sb = new StringBuilder();
			foreach(string alias in this.names) {
				sb.Append(alias);
			}
			return sb.ToString().GetHashCode();
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
		public Option AddParameter(SimpleParameter param)
		{
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
		public Option AddFlag(OptionFlags flag)
		{
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
		public bool HasFlag(OptionFlags flag)
		{
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
		public Option SetDefaultValue(string obj)
		{
			this.defaultValue = obj;
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
		public Option SetGroup(int g)
		{
			this.group = g;
			return this;
		}
		
		/// <summary>
		/// Removes all parameters set to this option.
		/// </summary>
		public void ClearState()
		{
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
		public Option SetAssert(TypeAssertionFlags flag, string defaultValue)
		{
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
		public Option AddAssertFlag(TypeAssertionFlags flag)
		{
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
		public bool HasAssertFlag(TypeAssertionFlags flag)
		{
			return (this.assertFlags & flag) == flag;
		}
		
		/// <summary>
		/// Asserts that the values of the parameters and the default value follows the indication
		/// of the assertion flags.
		/// </summary>
		public void AssertValues()
		{
			throw new NotImplementedException("Not implemented. Developer run out of free time :(");
		}
		
		/// <summary>
		/// Sets the mehtod name that the caller can use to invoke a method when the 
		/// option is used
		/// </summary>
		/// <param name="methodName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="owner">Instance/type of the object that has defined the method</param>
		/// <remarks>
		/// If the owner is an instance the method is assumed to be an instance method, but if the
		/// owner is a type the method is assumed to be an static one.
		/// </remarks>
		/// <returns>
		/// A <see cref="Option"/>
		/// </returns>
		public Option SetCallerMethodName(string methodName, object owner)
		{
			this.caller.MethodName = methodName;
			return this;
		}
		
		/// <summary>
		/// Sets the method delegate that will be invoked when this option is used
		/// in the command line
		/// </summary>
		/// <param name="deleg">
		/// A <see cref="OptionActionDelegate"/>
		/// </param>
		public Option SetCallerMethodDelegate(OptionActionDelegate deleg)
		{
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
		public Option SetDescription(string desc)
		{
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
		public string GetValue()
		{
			if (this.parameters.Count > 0)
				return this.parameters[0].Value;
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
		public string[] GetValues()
		{
			return this.Values;
		}
	}
}
