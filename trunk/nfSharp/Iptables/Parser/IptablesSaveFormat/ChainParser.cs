//  
//  Copyright (C) 2010 SharpKnocking project
//  File created by mangelp
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 

using System;
using NFSharp.Iptables.Core;
using CommonUtilities.Types;

namespace NFSharp.Iptables.Parser.IptablesSaveFormat {

	/// <summary>
	/// Parses a chain from a line
	/// </summary>
	public class ChainParser
	{
		public ChainParser () {
		}

		/// <summary>
		/// Gets if the string could be a chain as specified in standard
		/// iptables configuration file (see iptables-save output format).
		/// </summary>
		public bool IsChain(string line) {
			return line.StartsWith(":");
		}
		
		/// <summary>
		/// Gets if the name matches a builtin chain and sets that in the output
		/// parameter.
		/// </summary>
		public bool IsBuiltinChain(string name, out BuiltInChains chain) {
			object obj = null;

			if(!TypeUtil.TryGetEnumValue(typeof(BuiltInChains),
			                                   name.Trim(), out obj))
			{
				chain = BuiltInChains.Input;
				return false;
			}

		    chain = (BuiltInChains)obj;
			return true;
		}

		/// <summary>
		/// Parses a string and builds a object that represent the chain found
		/// in the line.
		/// </summary>
		public NetfilterChain ParseChain(string line, NetfilterTable table)
		{
			if(!IsChain(line)) {
				return null;
			}

			string[] parts = StringUtil.Split(line, true,':',' ','[',',',']');

			BuiltInChains blt;
			string name;
			RuleTargets defaultTarget;
			NetfilterChain ch = null;
			int pCount;
			int bCount;

			//First check if the table is builtin to get default target or not
			if(IsBuiltinChain(parts[0], out blt)) {
				if(parts.Length !=4 ) {
					throw new FormatException("The input string is not in iptables-save format. Bad number of parts.\nError in line: "+line);
				}

				defaultTarget = (RuleTargets)TypeUtil.GetEnumValue(typeof(RuleTargets),
				                                                   parts[1]);
				ch = new NetfilterChain(table, blt, defaultTarget);
				if( !Int32.TryParse(parts[2], out pCount) || !Int32.TryParse(parts[3],out bCount))
										throw new FormatException("The input string is not in iptables-save format. Bad integers.\nError in line: "+line);
				ch.SetCounters(pCount, bCount);
				return ch;
		    } else {
		        //Is not predefined. Grab the name and the counters
				if(parts.Length!=4 || parts[1]!="-")
					throw new FormatException("The input string is not in iptables-save format. Bad number of parts.\nError in line: "+line);
		        name = parts[0];
				if( !Int32.TryParse(parts[2], out pCount) || !Int32.TryParse(parts[3],out bCount))
										throw new FormatException("The input string is not in iptables-save format. Bad integers.\nError in line: "+line);
				
				ch = new NetfilterChain(table, name);
				ch.SetCounters(pCount, bCount);
		    }
			
			return ch;
		}
	}
}
