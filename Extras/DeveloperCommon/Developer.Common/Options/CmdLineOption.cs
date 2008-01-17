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

namespace Developer.Common.Options
{
	
	/// <summary>
	/// Option found when parsing a command line
	/// </summary>
	public class CmdLineOption: IComparable<CmdLineOption>
	{
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
		
		private string methodName;
		
		/// <summary>
		/// Name of a method that can process the option
		/// </summary>
		public string MethodName
		{
			get { return this.methodName; }
			set { this.methodName = value; }
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
			
			this.name = name;
			this.isLong = name.Length>1;
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
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException("The name of the option can't be null or empty");
			
			this.name = name;
			this.isLong = name.Length>1;
			this.procesingDelegate = procesingDelegate;
			this.methodName = String.Empty;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the option
		/// </param>
		/// <param name="isLong">
		/// A <see cref="System.Boolean"/> with the type of the option (long/short)
		/// </param>
		/// <param name="procesingDelegate">
		/// A <see cref="OptionProcessingDelegate"/> that will be called when the option
		/// matches a parameter
		/// </param>
		public CmdLineOption(string name, OptionProcessingDelegate procesingDelegate, bool isLong)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException("The name of the option can't be null or empty");
			
			this.name = name;
			this.isLong = isLong;
			this.procesingDelegate = procesingDelegate;
			this.methodName = String.Empty;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name for the option
		/// </param>
		/// <param name="method">
		/// A <see cref="System.String"/> with the name of a method that can handle
		/// this option when procesed.
		/// </param>
		public CmdLineOption(string name, string method)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException("The name of the option can't be null or empty");
			
			this.name = name;
			this.isLong = name.Length > 1;
			this.methodName = method;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">
		/// A <see cref="System.String"/> with the name of the option
		/// </param>
		/// <param name="method">
		/// A <see cref="System.String"/> with the name of the method
		/// </param>
		/// <param name="isLong">
		/// A <see cref="System.Boolean"/> with the option type (long/short)
		/// </param>
		public CmdLineOption(string name, string method, bool isLong)
		{
			if(String.IsNullOrEmpty(name))
				throw new ArgumentException("The name of the option can't be null or empty");
			
			this.name = name;
			this.isLong = isLong;
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
	}
}
