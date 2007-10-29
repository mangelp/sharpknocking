// /home/mangelp/Projects/sharpknocking/IptablesNet/IptablesNet.Core/Iptables/IIptablesAccessProxy.cs created with MonoDevelop
// User: mangelp at 13:58 23/05/2007
//
// Operations to send commands to the firewall software.
//
// Licensed under LGPL V2 (c) 2007 Luis Román y Miguel A. Pérez
//

using System;

using IptablesNet.Core;
using IptablesNet.Core.Commands;

namespace IptablesNet.Core.Iptables
{
	/// <summary>
	/// Methods that interfaces with the firewall rules in a simplistic way.
	/// </summary>
	public interface IIptablesAdapter
	{
		/// <summary>
		/// Applies a command over the current set of rules.
		/// </summary>
		void Exec(IIptablesCommand cmd);
		
		/// <summary>
		/// Returns the current set of rules in the tables of
		/// the firewall.
		/// </summary>
		NetfilterTableSet GetCurrentRuleSet();
		
		/// <summary>
		/// Alters the full set of rules in the firewall chains overwriting them.
		/// </summary>
		void SetCurrentRuleSet(NetfilterTableSet ruleSet);
		
		/// <summary>
		/// Returns a new transaction to execute commands in a atomic way.
		/// </summary>
		IIptablesTransaction CreateTransaction();
	}
}
