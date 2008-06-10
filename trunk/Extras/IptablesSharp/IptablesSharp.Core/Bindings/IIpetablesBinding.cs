// IIpetablesBinding.cs
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
using IptablesSharp.Core;

namespace IptablesSharp.Core.Bindings
{
	
	/// <summary>
	/// Methods to interact with concrete iptables bindings
	/// </summary>
	public interface IIpetablesBinding
	{
		string[] GetTableSet();
		void UpdateTableSet(NetfilterTableSet tableSet);
		void ApplyTableSet(NetfilterTableSet tableSet);
		NetfilterTable GetTable(string name);
		void UpdateTable(NetfilterTable table);
		void ApplyTable(NetfilterTable table);
		void ApplyRule(NetfilterRule rule);
		bool HasRule(NetfilterRule rule, PacketTableType table);
		bool HasTable(string name);
		int GetRuleCount(string name);
	}
}
