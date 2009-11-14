// Definitions.cs
//
//  Copyright (C)  2007 iSharpKnocking project
//  Created by Miguel Angel Perez Valencia, mangelp@gmail.com
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

using System;

using Developer.Common.Types;

namespace IptablesSharp.Core
{   
    /// <summary>
    /// Built in chains in iptables. Not all the builtin chains are applicable
    /// to every table. Each table contains a subset of builtin chains.
    /// </summary>
	/// <remarks>
	/// <see>PacketTableType for reference of available tables</see> 
	/// </remarks>
    public enum BuiltInChains:short
    {
        /// <summary>
		/// User defined chain. Can be in any table. 
		/// </summary>
        UserDefined=0,
		/// <summary>
		/// Incoming packets chain. Can be in Filter and Mangler tables.
		/// </summary>
        Input,
        /// <summary>
		/// Output packets chain. Can be in Nat, Mangler and Raw tables.
		/// </summary>
        Output,
        /// <summary>
		/// Forward packets chain. Can be in Filter and Mangler tables.
		/// </summary>
        Forward,
        /// <summary>
		/// Prerouting packets chain. Can be in Nat, Mangler and Raw tables.
		/// </summary>
        Prerouting,
        /// <summary>
		/// Postrouting packets chain. Can be in Nat and Mangler tables.
		/// </summary>
        Postrouting
    }
    
    /// <summary>
    /// Custom targets.
    /// </summary>
    public enum CustomRuleTargets
    {
        /// <summary>
        /// No custom target defined. The target is a predefined one.
        /// </summary>
        None=0,
        /// <summary>
        /// The target is a chain defined by user.
        /// </summary>
        UserDefinedChain=1,
        /// <summary>
        /// The target is a custom extension.
        /// </summary>
        CustomExtension=2
    }
    
    /// <summary>
    /// Targets for iptables rules. 
    /// </summary>
    /// <remarks>
    /// Here we have the predefined chains and also a value to specify that
    /// a user-defined chain have to be used (UserDefined).
    /// </remarks>
    public enum RuleTargets
    {
        /// <summary>
        /// The target can be a built-in chain, a user-defined chain or
        /// a target added to iptables as an extension.
        /// </summary>
        CustomTarget=0,
        /// <summary>
        /// Let the packet go in
        /// </summary>
        [Alias("ACCEPT")]
        Accept=1,
        /// <summary>
        /// Drop the packet out of any chains
        /// </summary>
        [Alias("DROP")]
        Drop=2,
        /// <summary>
        /// Pass the packet to userspace
        /// </summary>
        /// <remarks>
        /// This is a special built-in target
        /// </remarks>
        [Alias("QUEUE")]
        Queue=3,
        /// <summary>
        /// Stop traversing the current chain and resume at the next rule in the
        /// previous chain.
        /// </summary>
        /// <remarks>
        /// This is a special built-in target
        /// </remarks>
        [Alias("RETURN")]
        Return=4
    }
    
    /// <summary>
    /// Builtin tables in iptables for packets.
    /// </summary>
    public enum PacketTableType
    { 
        /// <summary>
        /// Default table. Contains built-in chains INPUT, FORWARD and OUTPUT.
        /// </summary>
        Filter,
        /// <summary>
        /// Table for packets that creates a new connection. It consists of
        /// three built-in chains: PREROUTING, OUTPUT, POSTROUTING.
        /// </summary>
        Nat,
        /// <summary>
        /// Table for specialized packet alteration. It contains built-in chains:
        /// PREROUTING, OUTPUT, INPUT, FORWARD, POSTROUTING.
        /// </summary>
        /// <remarks>
        /// The first two chains exists since kernel 2.4.17 and the rest where
        /// added since kernel 2.4.18
        /// </remarks>
        Mangler,
        /// <summary>
        /// Table for exemptions from connection tracking in combination with the NOTRACK target.
        /// It provides the built-in chains PREROUTING and OUTPUT. 
        /// </summary>
        Raw
    }
    
    /// <summary>
    /// Commands available
    /// </summary>
    public enum RuleCommands: short
    {
        /// <summary>
        /// Default value. None selected.
        /// </summary>
        None=0,
        /// <summary>
        /// List all rules in chain
        /// </summary>
        [Alias("list","L")]
        ListChain=(short)'L',
        /// <summary>
        /// Flush chain or all chains of current table
        /// </summary>
        [Alias("flush","F")]
        FlushChain=(short)'F',
        /// <summary>
        /// Zero the packet and byte count of all chains (?)
        /// </summary>
        [Alias("zero","Z")]
        ZeroChain=(short)'Z',
        /// <summary>
        /// Add a new chain to the current table
        /// </summary>
        [Alias("new-chain","N")]
        NewChain=(short)'N',
        /// <summary>
        /// Deletes a user-defined chain or all user-defined chains.
        /// </summary>
        [Alias("delete-chain","X")]
        DeleteChain=(short)'X',
        /// <summary>
        /// Rename a user-defined chain
        /// </summary>
        [Alias("rename-chain","E")]
        RenameChain=(short)'E',
        /// <summary>
        /// Set chain policy to target. Only for no user-defined chains.
        /// </summary>
        [Alias("policy","P")]
        SetChainPolicy=(short)'P',
        /// <summary>
        /// Append rule(s) to the end of chain or redirect chains
        /// </summary>
        [Alias("append","A")]
        AppendRule=(short)'A',
        /// <summary>
        /// Delete rule(s) from chain
        /// </summary>
        [Alias("delete","D")]
        DeleteRule=(short)'D',
        /// <summary>
        /// Insert rule(s) in chain
        /// </summary>
        [Alias("insert", "I")]
        InsertRule=(short)'I',
        /// <summary>
        /// Replace rule in chain
        /// </summary>
        [Alias("replace", "R")]
        ReplaceRule=(short)'R'
    }
    
    /// <summary>
    /// Options available to specify with any action
    /// </summary>
    public enum RuleOptions:short
    {
        /// <summary>
        /// Default value. None selected.
        /// </summary>
        None=0,
        /// <summary>
        /// Protocol of the rule or of the packet to check. 
        /// </summary>
        [Alias("protocol","p")]
        Protocol=(short)'p',
        /// <summary>
        /// Source address specification.
        /// </summary>
        [Alias("source","src","s")]
        Source=(short)'s',
        /// <summary>
        /// Destination address specification.
        /// </summary>
        [Alias("destination","dst","d")]
        Destination=(short)'d',
        /// <summary>
        /// Target for the rule when it is matched by a packet.
        /// </summary>
        [Alias("jump","j")]
        Jump=(short)'j',
        /// <summary>
        /// Processing should continue in a user-defined chain.
        /// </summary>
        [Alias("goto","g")]
        Goto=(short)'g',
        /// <summary>
        /// Interface via which a packet was received (only for Input, Fordward
        /// and Prerouting chains)
        /// </summary>
        [Alias("in-interface","i")]
        InInterface=(short)'i',
        /// <summary>
        /// Interface via which a packet is going to be sent (only for Fordward,
        /// Output and Postrouting chains).
        /// </summary>
        [Alias("out-interface","o")]
        OutInterface=(short)'o',
        /// <summary>
        /// The rule only refers to the next fragments of the packet (only for
        /// fragmented packets).
        /// </summary>
        [Alias("fragment","f")]
        Fragment=(short)'f',
        /// <summary>
        /// Change the packet and byte counters of a rule (Input, Append and
        /// Replace actions only).
        /// </summary>
        [Alias("set-counters","c")]
        SetCounters=(short)'c',
        /// <summary>
        /// Extended packet matching modules loaded explicitly.
        /// </summary>
        [Alias("match", "m")]
        MatchExtension=(short)'m'
    }
	
	public enum StringFormatOptions
	{
		/// <summary>
		/// Default formatting
		/// </summary>
		Default=0,
		/// <summary>
		/// Format compatible with iptables-restore supported format
		/// </summary>
		Iptables=1
	}
}
 
