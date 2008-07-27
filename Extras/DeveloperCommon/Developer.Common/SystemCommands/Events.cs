// Events.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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


using System;
using System.Security;

namespace Developer.Common.SystemCommands
{
	/// <summary>
	/// Arguments for the CommandEnd event
	/// </summary>
	public class CommandEndEventArgs: EventArgs
	{
		private CommandResult result;
		
		/// <summary>
		/// Result of the command
		/// </summary>
		public CommandResult Result
		{
			get {return this.result;}
			set {this.result = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="result">
		/// A <see cref="CommandResult"/>
		/// </param>
		public CommandEndEventArgs(CommandResult result)
		{
			this.result = result;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="exitCode">
		/// A <see cref="System.Int32"/>
		/// </param>
		public CommandEndEventArgs(int exitCode)
		{
			result = new CommandResult();
			result.ExitCode = exitCode;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="aborted">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public CommandEndEventArgs(bool aborted)
		{
			result = new CommandResult();
			result.Aborted = aborted;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="userData">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="detail">
		/// A <see cref="System.String"/>
		/// </param>
		public CommandEndEventArgs(object userData, string detail)
		{
			result = new CommandResult();
			result.UserData = userData;
			result.Detail = detail;
		}
	}

	/// <summary>
	/// Arguments for output read operations
	/// </summary>
	public class OutputReadEventArgs: EventArgs
	{
		private string data;
		
		/// <summary>
		/// Data read
		/// </summary>
		public string Data
		{
			get {return this.data;}
			set {this.data = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="data">
		/// A <see cref="System.String"/>
		/// </param>
		public OutputReadEventArgs(string data)
		{
			this.data = data;
		}
	}
	
	/// <summary>
	/// Arguments for authentication event handling/notification
	/// </summary>
	public class AuthRequiredEventArgs: EventArgs
	{
		private SecureString password;
		
		/// <summary>
		/// Auth password for the requested user
		/// </summary>
		public SecureString Password
		{
			get {return password;}
			set { password = value;}
		}
		
		private string userName;
		
		/// <summary>
		/// Gets the user name
		/// </summary>
		public string UserName
		{
			get {return userName;}
			set {this.userName = value;}
		}
		
		private string cmdName;
		
		/// <summary>
		/// Gets/sets the name of the command to execute
		/// </summary>
		public string CmdName
		{
			get {return cmdName;}
			internal set {this.cmdName = value;}
		}
		
		private bool success;
		
		/// <summary>
		/// Gets/sets if the authentication was successfull
		/// </summary>
		public bool Success
		{
			get {
				return this.success;
			}
			
			set {
				this.success = value;
			}
		}
		
		private bool delayed;
		
		/// <summary>
		/// Gets/Sets if the auth have been delayed. Usefull if it will be
		/// done later or is already done.
		/// </summary>
		public bool Delayed
		{
			get {
				return this.delayed;
			}
			
			set {
				this.delayed = value;
			}
		}
		
		/// <summary>
		/// Constructor. Initiallizes the command name and the user name.
		/// </summary>
		/// <param name="cmdName">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="userName">
		/// A <see cref="System.String"/>
		/// </param>
		public AuthRequiredEventArgs(string cmdName, string userName)
			:base()
		{
			this.password = new SecureString();
			this.userName = userName;
			this.cmdName = cmdName;
		}
		
		/// <summary>
		/// Constructor. Only initiallizes the command name.
		/// </summary>
		/// <param name="cmdName">
		/// A <see cref="System.String"/>
		/// </param>
		public AuthRequiredEventArgs(string cmdName)
			:this(cmdName, String.Empty)
		{
		}
	}
}
