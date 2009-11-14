// Main.cs
//
//  Copyright (C) SharpKnocking Project 2007
//  Author: ${author}
//  For a list of contributors see AUTHORS
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
// project created on 23/09/2007 at 15:15
using System;
using Gtk;

using Developer.Common.Options;

namespace SharpKnocking.Knocker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			
			CmdLineOptionsParser clop = new CmdLineOptionsParser(typeof(MainClass));
			clop.AddOptionWithMethod("help", "Help", "h");
			clop.ProcessParameters(args);
			
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
		
		public static void Help(OptionCallData data)
		{
			Console.WriteLine("iSharpKnocking port knocker. (c) 2007,2008 iSharpKnocking project");
			Console.WriteLine("This application is under heavy development and not fully");
			Console.WriteLine("functional.");
			System.Threading.Thread.CurrentThread.Abort();
		}
	}
}