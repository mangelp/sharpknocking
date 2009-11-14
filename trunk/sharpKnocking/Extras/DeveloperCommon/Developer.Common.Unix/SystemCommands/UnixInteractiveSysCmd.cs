// UnixInteractiveSysCmd.cs
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

namespace Developer.Common.Unix.SystemCommands
{
	public class UnixInteractiveSysCmd: BaseUnixSysCmd
	{
		/// <summary>
		/// Gets if the command can be executed synchronously
		/// </summary>
		public override bool CanExec {
			get { return false; }
		}

		/// <summary>
		/// Gets if the command can be executed asynchronously
		/// </summary>
		public override bool CanExecAsync {
			get { return true; }
		}
		
		/// <summary>
		/// Gets if the command can be executed asynchronously
		/// </summary>
		public override bool CanReadError {
			get { return true; }
		}

		/// <summary>
		/// Gets if the command can read from stdout
		/// </summary>
		public override bool CanRead {
			get { return true; }
		}

		/// <summary>
		/// Gets if the command can write to stdin
		/// </summary>
		public override bool CanWrite {
			get { return true; }
		}

		/// <summary>
		/// Constructor. Initiallizes the command name.
		/// </summary>
		/// <param name="cmd">
		/// A <see cref="System.String"/>
		/// </param>
		public UnixInteractiveSysCmd(string cmd)
			:base(cmd)
		{
		}
		
		/// <summary>
		/// Constructor. Initiallizes the command name and a boolean that says
		/// if this command must be executed as root.
		/// </summary>
		/// <param name="cmd">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="requireRoot">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public UnixInteractiveSysCmd(string cmd, bool requireRoot)
			:base(cmd, requireRoot)
		{
		
		}
	}
}
