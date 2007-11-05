// project created on 21/10/2007 at 13:25
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