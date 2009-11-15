// CmdLineOption.cs
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

namespace NFSharp.Iptables.Parser.IptablesSaveFormat
{
	
	/// <summary>
	/// Option description.
	/// </summary>
	public class CmdLineOption: IComparable<CmdLineOption>
	{
		/// <summary>
		/// Delegate to execute every time this option is found
		/// </summary>
		private OptionProcessingDelegate procesingDelegate;
		
		/// <summary>
		/// Delegate that will be executed to perform custom processing of the
		/// option.
		/// </summary>
		public OptionProcessingDelegate ProcesingDelegate
		{
			get { return this.procesingDelegate;}
			set { this.procesingDelegate = value;}
		}
		
		private bool allowMultiple;
		
		/// <summary>
		/// Gets/Sets if this option can be specified in the command line more than
		/// one time.
		/// </summary>
		public bool AllowMultiple
		{
			get {
				return this.allowMultiple;
			}
			set {
				this.allowMultiple = value;
			}
		}
		
		private string methodName;
		
		/// <summary>
		/// Name of a method that can process the option
		/// </summary>
		public string MethodName
		{
			get { return this.methodName; }
			set { this.methodName = value; }
		}
		
		private bool optionalValue;
		
		/// <summary>
		/// Gets if the value to this option is optional
		/// </summary>
		public bool OptionalValue
		{
			get {
				return this.optionalValue;
			}
		}
		
		/// <summary>
		/// Gets if this option has a value
		/// </summary>
		public bool HasValue
		{
			get {
				return this.parameter != null && 
					!String.IsNullOrEmpty(this.parameter.Value);
			}
		}
		
		private string name;
		
		/// <summary>
		/// Name of the option (case-sensitive)
		/// </summary>
		public string Name
		{
			get { return this.name; }
		}
		
		private bool isLong;
		
		/// <summary>
		/// Option type
		/// </summary>
		public bool IsLong
		{
			get { return this.isLong; }
		}
		
		private bool wasHit;
		
		/// <summary>
		/// Gets if this option was specified
		/// </summary>
		public bool WasHit {
			get {
				return wasHit;
			}
		}
		
		private int hitNumber;
		
		/// <summary>
		/// Gets the number of times this option was specified
		/// </summary>
		public int HitNumber {
			get {
				return hitNumber;
			}
		}
		
		private int hitPos;
		
		/// <summary>
		/// Gets the last position where a option was last found
		/// </summary>
		public int HitPosition
		{
			get {
				return this.hitPos;
			}
		}
		
		private SimpleParameter parameter;
		
		/// <summary>
		/// Parameter found in the command line that matches this option.
		/// This is the last parameter found if there was more than one.
		/// </summary>
		public SimpleParameter Parameter
		{
			get { return parameter;}
		}
		
		private string[] aliases;
		
		/// <summary>
		/// Array of name aliases
		/// </summary>
		public string[] Aliases{
			get {
				return this.aliases;
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name for the option
		/// </param>
		public CmdLineOption(string name)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException("The name of the option can't be null or empty");
			this.aliases = new string[0];
			this.name = name;
			this.isLong = name.Length>1;
			this.optionalValue = false;
			this.hitNumber = -1;
			this.hitPos = -1;
			this.allowMultiple = false;
			this.procesingDelegate = null;
			this.methodName = String.Empty;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name for the option
		/// </param>
		/// <param name="procesingDelegate">
		/// A <see cref="OptionProcessingDelegate"/> that will be called when the option
		/// is procesed
		/// </param>
		public CmdLineOption(string name, OptionProcessingDelegate procesingDelegate)
			:this(name)
		{
			this.procesingDelegate = procesingDelegate;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name for the option
		/// </param>
		/// <param name="method">
		/// A <see cref="System.String"/> with the name of a method that can handle
		/// this option when processed.
		/// </param>
		public CmdLineOption(string name, string method)
			:this(name)
		{
			this.methodName = method;
		}
		
		/// <summary>
		/// Compares two CmdLineOption objects by its name
		/// </summary>
		/// <param name="opt">
		/// A <see cref="CmdLineOption"/> With the option to compare to
		/// </param>
		/// <returns>
		/// A <see cref="System.Int32"/> greater than 0 if the former is greater, 
		/// less than 0 if the former is less and 0 if the names are equal.
		/// </returns>
		public int CompareTo(CmdLineOption opt)
		{
			return this.name.CompareTo(opt.name);
		}
		
		/// <summary>
		/// Sets the parameter value and it also sets the counter of hit number.
		/// </summary>
		/// <param name="sp">
		/// A <see cref="SimpleParameter"/>
		/// </param>
		/// <param name="order">Positon of the parameter in the input array of arguments</param>
		public void SetParameter(SimpleParameter sp, int order)
		{
			this.parameter = sp;
			this.hitNumber ++;
			this.wasHit = true;
			this.hitPos = order;
			if (this.hitNumber > 1 && !this.allowMultiple)
				throw new Exception("The option " + this.name + " can't be repeated.");
		}
		
		/// <summary>
		/// Sets the array as the command aliases
		/// </summary>
		/// <param name="aliases">
		/// A <see cref="System.String"/>
		/// </param>
		public void SetAliases(params string[] aliases)
		{
			this.aliases = aliases;
		}
		
		/// <summary>
		/// Determines if the name is an alias of this option
		/// </summary>
		/// <param name="names">
		/// A <see cref="System.String"/> name to check
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/> true if the name is equal to any alias or to
		/// the option default name
		/// </returns>
		public bool IsAlias(params string[] names)
		{
			foreach(string name in names) {
				if (String.Equals(name, this.name))
					return true;
				foreach (string str in this.aliases) {
					if (String.Equals(str, name))
						return true;
				}
			}
			return false;
		}
	}
}
