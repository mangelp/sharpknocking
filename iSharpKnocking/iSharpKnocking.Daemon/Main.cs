// /home/mangelp/Projects/sharpknocking/SharpKnocking/SharpKnocking.Daemon/Main.cs created with MonoDevelop at 17:20 14/06/2007 by mangelp 
//
//This project is released under the terms of the LGPL V2. See the file lgpl.txt for details.
//(c) 2007 SharpKnocking projects and authors (see AUTHORS).
// project created on 14/06/2007 at 17:20
using System;

using Developer.Common;
using Developer.Common.Options;

namespace SharpKnocking.Daemon
{
	class MainClass
	{
		/// <summary>
		/// Main class
		/// </summary>
		/// <param name="args">
		/// A <see cref="System.String"/>
		/// </param>
		public static void Main(string[] args)
		{
			CmdLineOptionsParser clop = new CmdLineOptionsParser(typeof(MainClass));
			clop.AddOptionWithMethod("help", "Help", "h");
			clop.ProcessParameters(args);
		}
		
		public static void Help(OptionCallData data)
		{
			Console.WriteLine("iSharpKnocking knocking daemon. (c) 2008 iSharpKnocking project");
			Console.WriteLine("This application is under heavy development and not fully");
			Console.WriteLine("functionall.");
		}
	}
}