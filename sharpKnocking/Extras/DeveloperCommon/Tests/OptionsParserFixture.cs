// MyClass.cs
//
//  Copyright (C) 2008 [name of author]
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
using NUnit;
using NUnit.Framework;
using NUnit.Core;

using Developer.Common.Options;

namespace Developer.Common.Tests
{
	
	[TestFixture]
	public class OptionsParserFixture
	{
		public class OwnerOne
		{
			public void TestMethod1()
			{
				Console.WriteLine("Test method from OwnerOne");
			}
			
			public static void TestMethod1Static()
			{
				Console.WriteLine("Test method static from OwnerOne");
			}
		}
		
		public class OwnerTwo
		{
			public void TestMethod2()
			{
				Console.WriteLine("Test method from OwnerTwo");
			}
			
			
			public static void TestMethod2Static()
			{
				Console.WriteLine("Test method static from OwnerTwo");
			}
		}
		
		OptionParser optParse;
		
		[TestFixtureSetUp]
		public void Initialize()
		{
			Console.WriteLine("Initializing");
			optParse = new OptionParser();
			optParse.ShowMessage = true;
		}
		
		public void EmptyHandler(OptionCallerData data)
		{
			Console.WriteLine("  Empty handler called for "+data.Parameter.Name
				+" with value "+data.Value);
		}
		
		public void HelpHandler(OptionCallerData data)
		{
			Console.WriteLine("  Help called");
		}
		
		private void ErrorHandler(object sender, OptionCallerData data)
		{
			Console.WriteLine("  ErrorHandler; Error: "+data.ErrorMessage);
		}
		
		private void OptionHandler(object sender, OptionCallerData data)
		{
			Console.WriteLine("  OptionHandler; Option alias " + data.Parameter.Name);
		}
		
		[Test]
		public void AddItems()
		{
			OwnerOne one = new OwnerOne();
			OwnerTwo two = new OwnerTwo();
			
			Console.WriteLine("Adding options");
			optParse.AddOption("EmptyHandler", OptionFlags.Required | OptionFlags.ValueRequired, "p")
				.SetDescription("Dumb option p")
				.Caller.Owner = this;
			optParse.AddOption("EmptyHandler", OptionFlags.ValueRequired, "mode")
				.SetDescription("Dumb option mode")
				.Caller.Owner = this;
			optParse.AddOption("EmptyHandler", OptionFlags.ValueRequired | OptionFlags.Negable, "b")
				.SetDescription("Dumb option aiefaoejf")
				.Caller.Owner = this;
			optParse.AddOption("EmptyHandler", OptionFlags.ValueRequired | OptionFlags.ExistingPath | OptionFlags.Multiple, "s")
				.SetDescription("Dumb optioneieieieie")
				.Caller.Owner = this;
			optParse.AddOption("HelpHandler", OptionFlags.DefaultOption, "h", "help")
				.SetDescription("Dumb option helepepepep")
				.Caller.Owner = this;
			optParse.AddOption("Number", OptionFlags.Defaults, "n", "number")
				.SetDescription("Number of elements")
				.SetDefaultValue("12")
				.AddAssertFlag(TypeAssertionFlags.Int)
				.Caller.Owner = this;
			
			optParse.AddOption("TestMethod1", OptionFlags.Defaults, "c1")
				.SetDescription("Call test method on owner 1")
				.Caller.Owner = one;
			
			optParse.AddOption("TestMethod2", OptionFlags.Defaults, "c2")
				.SetDescription("Call test method on owner 2")
				.Caller.Owner = two;
			
			optParse.AddOption("TestMethod1Static", OptionFlags.Defaults, "c1s")
				.SetDescription("Call test method on owner 1")
				.Caller.Owner = one.GetType();
			
			optParse.AddOption("TestMethod2Static", OptionFlags.Defaults, "c2s")
				.SetDescription("Call test method on owner 2")
				.Caller.Owner = two.GetType();
			
			optParse.AddOption("FooMethod", OptionFlags.Defaults, "peta")
				.SetDescription("Throws an exception because there is no owner")
				.Caller.Owner = null;
			
			Assert.IsNotNull(optParse.DefaultOption, "The default option was not set properly");
			optParse.ErrorFound += new EventHandler<OptionCallerData>(this.ErrorHandler);
			optParse.OptionFound += new EventHandler<OptionCallerData>(this.OptionHandler);
		}
		
		[Test]
		public void CheckDefaultOptionValue()
		{
			string val = optParse["number"].GetValue();
			Assert.AreEqual(val, "12");
		}
		
		[Test]
		public void CheckFailWithSecondDefaultOption()
		{
			try {
				optParse.AddOption("HelpHandler", OptionFlags.DefaultOption, "h2", "help2");
			} catch(OptionParserException ex) {
				Console.WriteLine("Correct exception found: "+ex.Message);
				return;
			}
			Assert.Fail("Failed when adding a second default option. It must throw OptionParserException");	
		}
		
		[Test()]
		public void CheckDuplicatedAliasFails()
		{
			bool ok = false;
			try {
				optParse.AddOption("HelpHandler", OptionFlags.DefaultOption, "p");
			} catch(ArgumentException ex) {
				ok = true;
				Console.WriteLine("Argument exception found: "+ex.Message);
			}
			if (!ok)
				Assert.Fail("Failed when adding an option with a duplicated alias. It must throw ArgumentException");			
		}
		
		[Test()]
		public void CheckHelpMessage()
		{
			Console.WriteLine("Showing autogenerated help");
			foreach(string str in optParse.GetOptionsDescription())
			{
				Console.WriteLine(str);
			}
		}
		
		[Test]
		public void CorrectEntryLineParsing()
		{
			Console.WriteLine("Checking well-formed entries");
			string args = 
				"-p 0 --mode ascii ! -b 2 -s /usr/local -s /usr/local/mono-svn";
			Console.WriteLine("Parsing => "+args);
			optParse.Parse(args);
			optParse.ProcessOptions();
		}
		
		[Test]
		public void CheckNumberOfOptions()
		{
			Option opt = optParse["p"];
			Assert.IsNotNull(opt, "Option 'p' added but can't be accessed");
			Assert.AreEqual(opt.ParamCount, 1);
			
			opt = optParse["mode"];
			Assert.IsNotNull(opt, "Option 'mode' added but can't be accessed");
			Assert.AreEqual(opt.ParamCount, 1);
			
			opt = optParse["b"];
			Assert.IsNotNull(opt, "Option 'b' added but can't be accessed");
			Assert.AreEqual(opt.ParamCount, 1);
			
			opt = optParse["s"];
			Assert.IsNotNull(opt, "Option 's' added but can't be accessed");
			Assert.AreEqual(opt.ParamCount, 2);
		}
		
		[Test]
		public void TestOtherOwnersCall()
		{
			Console.WriteLine("Checking options with other owners");
			string args = "--c1 --c2 --c1s --c2s";
			optParse.Parse(args);
			Console.WriteLine("Checking parsing");
			Assert.IsNotNull(optParse["c1"]);
			Assert.IsNotNull(optParse["c2"]);
			Assert.IsNotNull(optParse["c1s"]);
			Assert.IsNotNull(optParse["c2s"]);
			Console.WriteLine("Checking procesing");
			optParse.ProcessOptions();
			Console.WriteLine("Options successfully procesed");
		}
		
		[Test]
		public void IncorrectEntryLineParsing()
		{
			string msg = "Checking bad-formed entries";
			string[] args = {
				"-p 0 --mode ascii ! -b 2 -s /usr/local/xxp2h9 -s /usr/local/mono-svn",
				"--mode ascii ! -b 2 -s /usr/local/mono-svn"
			};
			this.TestIt(msg, args);
		}
		
		private void TestIt(string message, params string[] args)
		{
			Console.WriteLine(message);
			foreach(string arg in args) {
				Console.WriteLine("[Parsing] "+arg);
				optParse.Parse(arg);
				optParse.ProcessOptions();
			}
		}
		
		[Test]
		public void EntryLineParsingFailsWhenNoOwnerForMethodSet()
		{
			Console.WriteLine("Checking that fails when the option has a method but no owner");
			string args = "--peta";
			optParse.Parse(args);
			try{
				optParse.ProcessOptions();
			} catch (InvalidOperationException ex) {
				Console.WriteLine("Correct exception found: " + ex.Message);
				return;
			}
			
			Assert.Fail("Expected exception not found: InvalidOperationException");
		}
	}
}
