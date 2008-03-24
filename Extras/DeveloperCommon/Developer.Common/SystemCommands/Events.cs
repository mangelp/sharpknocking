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
}
