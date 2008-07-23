// SystemCommandsFixture.cs
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

using NUnit.Core;
using NUnit.Framework;

using Developer.Common.SystemCommands;

namespace Developer.Common.Tests
{
	
	[TestFixture]
	public class SystemCommandsFixture
	{
		public SystemCommandsFixture()
		{
		}
		
		[TestFixtureSetUp]
		public void SetupFixture()
		{}
		
		[Test]
		public void ExecWhich()
		{
			Console.WriteLine("Executing command 'which mono'");
			TextOutputCommand cmd = new TextOutputCommand("which");
			cmd.OutputRead += new EventHandler<OutputReadEventArgs>(this.TextRead);
			cmd.SyncReadMode = ReadMode.All;
			
			cmd.Args = "mono";
			Console.WriteLine("Executing command "+cmd.Name);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.Args = "monodevelop";
			Console.WriteLine("Executing command "+cmd.Name);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.SyncReadMode = ReadMode.Line;
			cmd.Args = "mono";
			Console.WriteLine("Executing command "+cmd.Name);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
			cmd.Args = "monodevelop";
			Console.WriteLine("Executing command "+cmd.Name);
			cmd.Exec();
			Console.WriteLine("Direct read "+cmd.Read());
		}
		
		private void TextRead(object sender, OutputReadEventArgs args)
		{
			Console.WriteLine("Result: "+args.Data);
		}
		
		[Test]
		public void ReadIptablesConfig()
		{
			TextOutputCommand toc = new TextOutputCommand("/sbin/itables-save", true);
			Console.WriteLine("Iptables-save output:\n"+toc.Read());
			Console.WriteLine("RC: "+toc.ExitCode);
		}
	}
}
