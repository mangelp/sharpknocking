// Events.cs
//
//  Copyright (C)  2007 SharpKnocking project
//  Created by ${Author}
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

namespace Developer.Common.SystemCommands
{
	public class CommandEndEventArgs: EventArgs
	{
		private CommandResult result;
		
		public CommandResult Result
		{
			get {return this.result;}
			set {this.result = value;}
		}
		
		public CommandEndEventArgs(CommandResult result)
		{
			this.result = result;
		}
		
		public CommandEndEventArgs(int exitCode)
		{
			result = new CommandResult();
			result.ExitCode = exitCode;
		}
		
		public CommandEndEventArgs(bool aborted)
		{
			result = new CommandResult();
			result.Aborted = aborted;
		}
		
		public CommandEndEventArgs(object userData, string detail)
		{
			result = new CommandResult();
			result.UserData = userData;
			result.Detail = detail;
		}
	}
}
