// GksuSysCmd.cs
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

using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	public class GksuSysCmd
	{
		private static readonly string CommandName = "gksu";
		private static readonly string ArgsTemplate = "{0} --message \"{2}\" \"{3} {4}\"";
		private static readonly string UserTemplate = "--user {0}";
		
		public GksuSysCmd()
		{
		}
		
		public static string GetCommandName()
		{
			return CommandName;
		}
		
		public static string GetArgsFor(string commandName, string commandArgs)
		{
			return GetArgsFor(commandName, commandArgs, String.Empty, 
			                  "Auth needed to run " + commandName + " as root", false);
		}
		
		public static string GetArgsFor(string commandName, string commandArgs, bool blockKeyboard)
		{
			return GetArgsFor(commandName, commandArgs, String.Empty, 
			                  "Auth needed to run " + commandName + " as root", blockKeyboard);
		}
		
		public static string GetArgsFor(string commandName, string commandArgs, string user, bool blockKeyboard)
		{
			return GetArgsFor(commandName, commandArgs, user, 
			                  "Auth needed to run " + commandName + " as " + user, blockKeyboard);
		}
		
		public static string GetArgsFor(string commandName, string commandArgs, string user, string msg, bool blockKeyboard)
		{
			return String.Format(ArgsTemplate,
			                     String.IsNullOrEmpty(user) ? String.Empty : String.Format(UserTemplate, user),
			                     msg,
			                     commandName,
			                     commandArgs);
		}
	}
}
