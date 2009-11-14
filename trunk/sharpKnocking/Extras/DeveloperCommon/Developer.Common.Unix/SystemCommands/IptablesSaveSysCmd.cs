// IptablesSaveSysCommand.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia (mangelp{aT}gmail[D0T]com)
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

using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	/// <summary>
	/// Wraps the command-line iptables-save command that allows to
	/// get a copy of the current iptables ruleset.
	/// </summary>
	public class IptablesSaveSysCmd: UnixTextOutputSysCmd
	{
		private static readonly string CommandName = "iptables-save";
		
		/// <summary>
		/// Constructor. Initializes the name of the command to iptables-save
		/// </summary>
		public IptablesSaveSysCmd()
			:base(CommandName, true)
		{
		}
		
		/// <summary>
		/// Saves the current set of rules to a file
		/// </summary>
		/// <param name="fileName">
		/// A <see cref="System.String"/>
		/// </param>
		public void ToFile(string fileName)
		{
			string text = base.Read();
			File.WriteAllText(fileName, text);
		}
	}
}
