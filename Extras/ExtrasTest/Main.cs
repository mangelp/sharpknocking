// Main.cs
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
using System.Collections;
using System.Collections.Generic;

using Developer.Common.SystemCommands;
using Developer.Common.Unix.SystemCommands;

using IptablesNet.Core;

namespace ExtrasTest
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Test2(args);
		}
		
		public static void Test2(string[] args)
		{
			NetfilterTableSet ts = new NetfilterTableSet();
			Console.WriteLine("#################################### Loading "+args[0]);
			ts.LoadFromFile(args[0]);
			Console.WriteLine("#################################### Saving "+args[1]);
			ts.SaveToFile(args[1], true);
//			ts = new NetfilterTableSet();
//			Console.WriteLine("#################################### Loading "+args[1]);
//			ts.LoadFromFile(args[1]);
//			Console.WriteLine("#################################### Saving "+args[1]);
//			ts.SaveToFile(args[1]+".temp", true);
		}
		
		public static void Test1(string[] args)
		{
			Console.Out.WriteLine("Searching mono in the path");
			WhichSysCmd cmd = new WhichSysCmd();
			cmd.Args = "mono";
			CommandResult cres = cmd.Exec();
			List<string> result = (List<string>)cres.UserData;
			
			if(result.Count == 0)
				Console.Out.WriteLine("There are no results");
			for(int i=0;i<result.Count;i++)
			{
				Console.Out.WriteLine("Result["+i+"]: "+result[i]);
			}
			
			Console.Out.WriteLine("ExitCode: "+cres.ExitCode);
			
			Console.Out.WriteLine("Locating libgtkembedmoz.so");
			LocateSysCmd cmd2 = new LocateSysCmd();
			cmd2.Args="libgtkembedmoz.so";
			cres = cmd2.Exec();
			result = (List<string>)cres.UserData;
			
			if(result.Count == 0)
				Console.Out.WriteLine("There are no results");
			for(int i=0;i<result.Count;i++)
			{
				Console.Out.WriteLine("Result["+i+"]: "+result[i]);
			}
			
			Console.Out.WriteLine("ExitCode: "+cres.ExitCode);		
		}
	}
}