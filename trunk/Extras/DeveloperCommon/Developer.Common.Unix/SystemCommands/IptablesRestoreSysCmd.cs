// IptablesRestoreSysCommand.cs
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
using System.Collections.Generic;

using Developer.Common.Options;
using Developer.Common.SystemCommands;

namespace Developer.Common.Unix.SystemCommands
{
	/// <summary>
	/// Wrapper to the system command that restores iptables rule set.
	/// </summary>
	/// <remarks>
	/// The man page for iptables-restore is outdated in fedora release 8 and only shows
	/// two options but executing the command with the --help switch shows this:
	/// <br/><br/>
	/// Usage: iptables-restore [-b] [-c] [-v] [-t] [-h]
	///           [ --binary ]
	///           [ --counters ]
	///           [ --verbose ]
	///           [ --test ]
	///           [ --help ]
	///           [ --noflush ]
	///           [ --table=<TABLE> ]
	///           [ --modprobe=<command>]
	/// <br/><br/>There is also a short option for --noflush named -n that shows in the man page
	/// but not here.
	/// <br/><br/>These switches are used to perform various operations like replacing all the 
	/// rules or adding/inseting new rules.
	/// </remarks>
	public class IptablesRestoreSysCmd: UnixTextInputSysCmd
	{
		private static OptionList<Option> options;
		private static string CommandName;
		private static string COMMIT;
		
		/// <summary>
		/// Initiallizes the list of available options for this command
		/// </summary>
		static IptablesRestoreSysCmd()
		{
			CommandName = "iptables-restore";
			COMMIT = "COMMIT";
			
			options = new OptionList<Option>();
			Option opt = new Option();
			opt.AddAliases("binary", "b");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("counters", "c");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("verbose", "v");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("test", "t");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("help", "h");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("noflush", "n");
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("table")
				.AddFlag(OptionFlags.ValueRequired)
				.AddAssertFlag(TypeAssertionFlags.String);
			options.Add(opt);
			
			opt = new Option();
			opt.AddAliases("modprobe")
				.AddFlag(OptionFlags.ValueRequired)
				.AddAssertFlag(TypeAssertionFlags.String);
			options.Add(opt);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public IptablesRestoreSysCmd()
			:base(CommandName, true)
		{
		}
		
		private void WriteLine(string str)
		{
			if (str.EndsWith("\n"))
				this.Write(str);
			else
				this.Write(str+"\n");
		}
		
		private void WriteRuleSet(IList<string> ruleset)
		{
			this.Exec();
			
			int pos = 0;
			
			while(this.IsRunning && pos < ruleset.Count) {
				this.WriteLine(ruleset[pos]);
			}
			
			//TODO: Should we throw an exception when the iptables-restore command fails?
			
			if (this.IsRunning) {
				this.WriteLine(COMMIT);
			}
			
			this.Stop();
		}
		
		/// <summary>
		/// Executes the rules over the current set of rules without flushing it or reseting
		/// counters.
		/// </summary>
		/// <param name="ruleset">
		/// A <see cref="IList`1"/>
		/// </param>
		/// <param name="test">
		/// A <see cref="System.Boolean"/>
		/// </param>
		/// <param name="verbose">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void ExecRules(IList<string> ruleset, bool test, bool verbose)
		{
			string oldArgs = this.Args;
			this.Args += options["noflush"];
			if (test)
				this.Args += options["test"];
			if (verbose)
				this.Args += options["verbose"];
			this.WriteRuleSet(ruleset);
			this.Args = oldArgs;
		}
		
		/// <summary>
		/// Executes the rules over the current set of rules without flushing it or reseting
		/// counters.
		/// </summary>
		/// <param name="ruleset">
		/// A <see cref="IList`1"/>
		/// </param>
		/// <param name="test">
		/// A <see cref="System.Boolean"/>
		/// </param>
		public void ExecRules(IList<string> ruleset, bool test)
		{
			this.ExecRules(ruleset, test, false);
		}
		
		/// <summary>
		/// Executes the rules over the current set of rules without flushing it or reseting
		/// counters.
		/// </summary>
		/// <param name="ruleset">
		/// A <see cref="IList`1"/>
		/// </param>
		public void ExecRules(IList<string> ruleset)
		{
			this.ExecRules(ruleset, false, false);
		}
	}
}
