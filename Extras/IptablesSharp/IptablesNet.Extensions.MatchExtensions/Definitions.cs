// Definitions.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by Miguel Angel Perez (mangelp{@}gmail{d0t}com)
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
using System.ComponentModel;

using Developer.Common.Types;

namespace IptablesNet.Extensions.Matches
{
	[FlagsAttribute()]
	public enum ConnectionStates
	{
	    //Default value with no real mean for iptables.
	    None=0,
	    //Packets that aren't recognized and icmp paquets from an unknown
	    //connection.
	    [AliasAttribute("INVALID")]
	    Invalid=1,
	    //The packet has started a new connection or is from a connection which
	    //packets have been seen in only one direction
	    [AliasAttribute("NEW")]
	    New=2,
	    //The packet belong to a connection which has seen packets in both
	    //directions
	    [AliasAttribute("ESTABLISHED")]
	    Established=4,
	    //The packet is starting a new connection but that connection is associated
	    //with an existing connection
	    [AliasAttribute("RELATED")]
	    Related=8
	}
	
	public enum IcmpMatchOptions
	{
	    //No need of None 
	    [AliasAttribute("icmp-type")]
	    IcmpType
	}
	
	public enum StateMatchOptions
	{
	    //No need of None 
	    [AliasAttribute("state")]
	    State
	}
	
	public enum UdpMatchOptions
	{
	    //Default value for initialization. Not valid for use.
	    None=0,
		/// <summary>
		/// Match source port for the packet.
		/// </summary>
	    [AliasAttribute("source-port","sport")]
	    SourcePort,
		/// <summary>
		/// Match destination port for the packet
		/// </summary>
	    [AliasAttribute("destination-port","dport")]
	    DestinationPort
	}
	
	public enum TcpMatchOptions
	{
	    //Default value for initialization. Not valid for use.
	    None=0,
		/// <summary>
		/// Match source port for the packet.
		/// </summary>
	    [AliasAttribute("source-port","sport")]
	    SourcePort,
		/// <summary>
		/// Match destination port for the packet
		/// </summary>
	    [AliasAttribute("destination-port","dport")]
	    DestinationPort,
		/// <summary>
		/// Match tcp flags
		/// </summary>
	    [AliasAttribute("tcp-flags")]
	    TcpFlags,
		/// <summary>
		/// Match syn flag
		/// </summary>
	    [AliasAttribute("syn")]
	    Syn,
		/// <summary>
		/// Match tcp option flags
		/// </summary>
	    [AliasAttribute("tcp-option")]
	    TcpOption
	    //This option is in the man page but not shown with the command
	    // iptables -m tcp -h
	    // v. 1.3.5 of iptables
        //[AliasAttribute("mss")]
        //Mss
	}
	
}
