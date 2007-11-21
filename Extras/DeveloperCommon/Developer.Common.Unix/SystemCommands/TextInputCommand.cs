// TextInputCommand.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{aT}gmail[D0T]com)
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
using System.Collections.Generic;

using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	/// <summary>
	/// Models a command that is mean to process the input as it comes
	/// </summary>
	public class TextInputCommand: BaseCommandWrapper
	{
		public override bool CanExec {
			get { return false; }
		}

		public override bool CanExecAsync {
			get { return true; }
		}

		public override bool CanRead {
			get { return false; }
		}

		public override bool CanWrite {
			get { return true; }
		}

		
		public TextInputCommand(string cmd)
			:base(cmd)
		{
		}
		
		public TextInputCommand(string cmd, bool requireRoot)
			:base(cmd, requireRoot)
		{
		
		}
		
		protected override Process GetNewProcess ()
		{
			Process p = base.GetNewProcess ();
			p.StartInfo.RedirectStandardInput = true;
			return p;
		}

		public override CommandResult Exec ()
		{
			throw new NotSupportedException();
		}
		
		public override void ExecAsync ()
		{
			//TODO: Support to this
		}
		
		public override void Abort ()
		{
			//TODO: Support to abort last write
		}

		public override void Write (string data)
		{
			//TODO: Support to write to process input
		}
	}
}
