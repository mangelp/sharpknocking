// BaseUnixSysCmd.cs
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
using System.Diagnostics;

using Developer.Common.Unix.Native;
using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	public abstract class BaseUnixSysCmd: BaseSystemCommand
	{
		public BaseUnixSysCmd(string name)
			:base(name, false)
		{
			
		}
		
		public BaseUnixSysCmd(string name, bool requiresRoot)
			:base(name, requiresRoot)
		{
			
		}
		
		public BaseUnixSysCmd(string name, string user)
			:base(name, user)
		{
			if (String.Equals(user, "root", StringComparison.InvariantCultureIgnoreCase))
				this.RequiresRoot = true;
		}
		
		protected override Process GetNewProcess ()
		{
			Process p = base.GetNewProcess ();
			
			if (this.RequiresRoot) {
				p.StartInfo.FileName = GksuSysCmd.GetCommandName();
				p.StartInfo.Arguments = GksuSysCmd.GetArgsFor(
				    this.Name,
					this.Args,
					true);
			} else if (!String.IsNullOrEmpty(this.ExecuteAs)
			           && UnixNative.IsCurrentUser(this.ExecuteAs)) {
				p.StartInfo.FileName = GksuSysCmd.GetCommandName();
				p.StartInfo.Arguments = GksuSysCmd.GetArgsFor(
				    this.Name,
					this.Args,
					this.ExecuteAs,
					true);
			}
			
			return p;
		}

	}
}
