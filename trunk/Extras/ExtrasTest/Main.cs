// project created on 21/10/2007 at 13:25
using System;
using System.Collections;
using System.Collections.Generic;

using Developer.Common.SystemCommands;
using Developer.Common.Unix.SystemCommands;

namespace ExtrasTest
{
	class MainClass
	{
		public static void Main(string[] args)
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