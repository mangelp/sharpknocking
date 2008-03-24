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
using System.Reflection;
using System.Collections.Generic;

using Developer.Common.Options;

namespace Test
{
	class MainClass
	{
		private static string testName = "";
		
		public static void Main(string[] args)
		{
			CmdLineOptionsParser clop = new CmdLineOptionsParser(typeof(MainClass));
			clop.AllowNoOptions = true;
			clop.DefaultOptionName = "help";
			clop.AddOptionWithMethod("test", "Testor", "t");
			clop.AddOptionWithMethod("help", "Helpor", "h");
			clop.ProcessParameters(args);
			CmdLineOption tstOpt = clop["test"];
			
			if (tstOpt.WasHit && tstOpt.HasValue) {
				int length = args.Length - tstOpt.HitPosition - 2;
				if (length < 0)
					length = 0;
				string[] nargs = new string[length];
				//Console.WriteLine("Copy from "+pos+" "+nargs.Length+" elements over "+args.Length+" elements ");
				if (length > 0)
					Array.Copy(args, tstOpt.HitPosition + 2, nargs, 0, length);
				DoTest(nargs);
			}
		}
		
		public static void Testor(OptionCallData data)
		{
			data.AbortParsing = true;
			MainClass.testName = "Test.Test"+data.Value;
			if (!data.SourceOption.HasValue)
				Console.WriteLine("You must specify a value for the parameter " 
				                  + data.SourceOption.Parameter.Name);
		}
		
		private static void DoTest(string[] args)
		{
			Assembly asm = Assembly.GetExecutingAssembly();
			Type[] types = asm.GetTypes();
			Type t = null;
			for (int i = 0; i<types.Length; i++) {
				if (types[i].FullName.Equals(MainClass.testName, StringComparison.InvariantCultureIgnoreCase)) {
					t = types[i];
					break;
				}
			}

			if (t == null) {
				Console.WriteLine("Can't load test "+MainClass.testName);
				return;
			}
			
			ITesteable instance = (ITesteable)Activator.CreateInstance(t);
			if(instance == null) {
				Console.WriteLine("Can't create test "+MainClass.testName);
				return;
			}
				
			try {
				instance.Test(args);
			} catch (Exception ex) {
				Console.WriteLine("Can't execute test. Reason: "+ ex.Message);
			}		
		}
		
		public static void Helpor(OptionCallData data)
		{
			Console.WriteLine("Test app (c) iSharpKnocking project.");
			Console.WriteLine("This app is used to test things. All options after the");
			Console.WriteLine("test argument are sent to the test");
			Console.WriteLine("Usage: ");
			Console.WriteLine(" --help|-h : Show help about the usage and a list of tests");
			Console.WriteLine(" --test|-t : Specify the name of the test to execute");
			Console.WriteLine("Tests available: (names are case-sensitive)");
			Console.WriteLine(" OptionsStatic");
			Console.WriteLine(" OptionsInstance");
			data.AbortParsing = true;
		}
	}
	
	public interface ITesteable
	{
		void Test(string[] args);
	}
	
	/// <summary>
	/// Test that uses static methods to handle options
	/// </summary>
	class TestOptionsStatic: ITesteable
	{
		public void Test(string[] args)
		{
			Console.WriteLine("Testing options static");
			CmdLineOptionsParser clop = new CmdLineOptionsParser(this.GetType());
			clop.DefaultOptionName = "Default";
			clop.AddOptionWithMethod("help", "Help", "h");
			clop.AddOption("verbose");
			clop.AddOptionWithMethod("k", "X", "kill");
			clop.AddOptionWithMethod("v", "V", "verbosity");
			clop.AddOptionWithMethod("Default", "DefaultOpt");
			clop.ProcessParameters(args);
		}
		
		public static void DefaultOpt(OptionCallData data)
		{
			Console.WriteLine("There are no options. Use -h for help.");
		}
		
		public static void Help(OptionCallData data)
		{
			Console.WriteLine("Test app. (c) 2008 Miguel Angel Perez");
			Console.WriteLine("This app is for test purposses only and doesn't"+
			                  ". Any functionallity can be tested here, but if"+
			                  " it is here it will surely be broken.");
			Console.WriteLine("Options available: ");
			Console.WriteLine(" -k|--kill: Ends option procesing");
			Console.WriteLine(" -v|--verbosity: Increases application verbosity");
			data.AbortParsing = true;
		}
		
		public static void X(OptionCallData data)
		{
			Console.WriteLine("This is x killing the processing");
			data.AbortParsing = true;
		}
		
		public static void V(OptionCallData data)
		{
			int level = 0;
			
			if (!data.SourceOption.HasValue) {
			    Console.WriteLine("The " + data.SourceOption.Parameter.Name + " requires an integer value");
				data.AbortParsing = true;
			} else if (!Int32.TryParse( data.Value, out level)) {
			    Console.WriteLine("The " + data.SourceOption.Parameter.Name + " requires an integer value");
				data.AbortParsing = true;
			} else {
				Console.WriteLine("Verbosity set to " + 
				                  (data.SourceOption.Parameter.Not ? "not " : String.Empty)
				                  + level);
			}
		}
	}
	
	/// <summary>
	/// Test with instance versions of the previous one to handle options
	/// </summary>
	class TestOptionsInstance: ITesteable
	{
		public void Test(string[] args)
		{
			Console.WriteLine("Testing options instance");
			if(args.Length==0)
				Console.WriteLine("There are no options. Use -h for help.");
			CmdLineOptionsParser clop = new CmdLineOptionsParser((object)this);
			clop.AddOptionWithMethod("help", "Help", "h");
			clop.AddOption("verbose");
			clop.AddOptionWithMethod("k", "X", "kill");
			clop.AddOptionWithMethod("v", "V", "verbosity");
			clop.ProcessParameters(args);
		}
		
		public void Help(OptionCallData data)
		{
			Console.WriteLine("Test app. (c) 2008 Miguel Angel Perez");
			Console.WriteLine("This app is for test purposses only and doesn't"+
			                  ". Any functionallity can be tested here, but if"+
			                  " it is here it will surely be broken.");
			Console.WriteLine("Options available: ");
			Console.WriteLine(" -k|--kill: Ends option procesing");
			Console.WriteLine(" -v|--verbosity: Increases application verbosity");
			data.AbortParsing = true;
		}
		
		public void X(OptionCallData data)
		{
			Console.WriteLine("This is x killing the processing");
			data.AbortParsing = true;
		}
		
		public void V(OptionCallData data)
		{
			int level = 0;
			
			if (String.IsNullOrEmpty(data.SourceOption.Parameter.Value)) {
			    data.ErrorMessage = "The "+data.SourceOption.Parameter.Name+" requires an integer value";
				data.AbortParsing = true;
			} else if (!Int32.TryParse( data.SourceOption.Parameter.Value, out level)) {
			    data.ErrorMessage = "The "+data.SourceOption.Parameter.Name+" requires an integer value";
				data.AbortParsing = true;
			} else {
				Console.WriteLine("Verbosity set to "+ 
				                  (data.SourceOption.Parameter.Not?" ! ":"")
				                  +level);
			}
		}
	}
}