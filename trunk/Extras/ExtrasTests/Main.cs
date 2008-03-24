// Main.cs
//
//  Copyright (C)  2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp_AT_gmail_DOT_com)
//
//  This library is free software; you can redistribute it and/or
//  modify it under the terms of the GNU Lesser General Public
//  License as published by the Free Software Foundation; either
//  version 2.1 of the License, or (at your option) any later version.
//
//  This library is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
//  Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public
//  License along with this library; if not, write to the Free Software
//  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
using System;
//using System.Collections;
//
using Developer.Common.Options;

namespace ExtrasTests
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			CmdLineOptionsParser optParser = new CmdLineOptionsParser();
			optParser.AddOption("help", true, new CmdLineOption.OptionProcesing(Help));
			optParser.AddOption("n", false, new CmdLineOption.OptionProcesing(Number));
			optParser.AddOption("file", true, new CmdLineOption.OptionProcesing(File));
			ble
		}
		
		public static void Help()
		{
			Console.WriteLine("Found help option: "+par);
		}
		
		public static void Number()
		{
			Console.WriteLine("Found Number option: "+par);		
		}
		
		public static void File()
		{
			Console.WriteLine("Found File option: "+par);		
		}
	}
}