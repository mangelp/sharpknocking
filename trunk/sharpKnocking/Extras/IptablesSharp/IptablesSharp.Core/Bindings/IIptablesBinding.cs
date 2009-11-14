// IIpetablesBinding.cs
//
//  Copyright (C) 2008 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp)at(gmail)dot(com)
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

using IptablesSharp.Core;

namespace IptablesSharp.Core.Bindings
{
	
	/// <summary>
	/// Methods to interact with concrete iptables bindings
	/// </summary>
	public interface IIptablesBinding
	{
		/// <summary>
		/// Gets the current output of the iptables-save command
		/// </summary>
		string GetCurrentTables();
		/// <summary>
		/// Saves the current output of the iptables-save command to
		/// a file.
		/// </summary>
		/// <param name="fileName">
		/// A <see cref="System.String"/>
		/// </param>
		void GetCurrentTables(string fileName);
		/// <summary>
		/// Sets the rules throught the iptables-restore command
		/// </summary>
		void SetTable(string table, IList<string> ruleset);
		/// <summary>
		/// Sets the rules througth the iptables-restore command
		/// </summary>
		/// <param name="fileName">
		/// Name of the file with the rule set in the proper format
		/// </param>
		void SetRuleset(IList<string> ruleset);
	}
}
