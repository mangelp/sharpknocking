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
		
		public BaseUnixSysCmd(string name, bool authRequired)
			:base(name, authRequired)
		{
			
		}
		
		public BaseUnixSysCmd(string name, string user)
			:base(name, user)
		{

		}
		
		protected override string GetAdminName()
		{
			return "root";
		}
		
		protected override void OnAuthRequired(AuthRequiredEventArgs args)
		{
			args.UserName = String.IsNullOrEmpty(this.ExecuteAs) ? 
				this.GetAdminName() : 
					this.ExecuteAs;
			
			Process p = this.Current;
			
			if (!UnixNative.IsCurrentUser(this.ExecuteAs)) {
				p.StartInfo.FileName = GksuSysCmd.GetCommandName();
				p.StartInfo.Arguments = GksuSysCmd.GetArgsFor(
				    this.CmdName,
					this.Args,
					args.UserName,
					true);
			}
			
			args.Delayed = true;
			args.Success = false;
		}

	}
}
