// Enumerations.cs
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

namespace Developer.Common.Net
{
	//TODO: The enumeration constants of TcpFlags and IcmpTypes should be commented
	//with the meaning of the flag/icmptype.
	
    /// <summary>
    /// Tcp flags.
    /// </summary>
    [FlagsAttribute()]
    public enum TcpFlags:byte
    {
        /// <summary>
        /// Default Value. No flags. This value is not real and is only for
        /// initialization. Don't use it directly, methods won't accept it.
        /// </summary>
        None=0,
        SYN=1,
        ACK=2,
        FIN=4,
        RST=8,
        URG=16,
        PSH=32,
        ALL=63
    }
    
    /// <summary>
    /// Types of icmp packets.
    /// <summary>
    /// <remarks>
    /// The Alias Attribute have been used to provide support for aliases and
    /// searching the original names instead of providing in another class
    /// methods to compare names.
    /// <br/>
    /// Every enumeration member has at least one alias name. The names used
    /// in the commands are the aliased ones
    /// <br/><br/>
    /// In linux you can obtain this list from iptables using this:
    ///      iptables -p icmp -h
    /// <br/><br/>
    /// The type numbers and codes are taken from:
    /// http://www.iana.org/assignments/icmp-parameters
    /// <br/>
    /// The numbers asigned to the constants is the number defined in the standard.
	/// For the subtypes of icmp the number asigned is the result of this calculation:
	///     icmpTypeNumber*100 + icmpSubtypeNumber
    /// </remarks>
	public enum IcmpTypes:ushort
	{
	    [AliasAttribute("echo-reply", "pong")]
	    EchoReply=0,
	    [AliasAttribute("destination-unreachable")]
	    DestinationUnreachable=3,
	       [AliasAttribute("network-unreachable")]
	       NetworkUnreachable=300,
	       [AliasAttribute("host-unreachable")]
	       HostUnreachable=301,
	       [AliasAttribute("protocol-unreachable")]
	       ProtocolUnreachable=302,
	       [AliasAttribute("port-unreachable")]
	       PortUnreachable=303,
	       [AliasAttribute("fragmentation-needed")]
	       FragmentationNeeded=304,
	       [AliasAttribute("source-route-failed")]
	       SourceRouteFailed=305,
	       [AliasAttribute("network-unknown")]
	       NetworkUnknown=306,
	       [AliasAttribute("host-unknown")]
	       HostUnknown=307,
	       [AliasAttribute("network-prohibited")]
	       NetworkProhibited=309,
	       [AliasAttribute("host-prohibited")]
	       HostProhibited=310,
	       [AliasAttribute("TOS-network-unreachable")]
	       TosNetworkUnreachable=311,
	       [AliasAttribute("TOS-host-unreachable")]
	       TosHostUnreachable=312,
	       [AliasAttribute("CommunicationProhibited")]
	       Communication_prohibited=313,
	       [AliasAttribute("host-precedence-violation")]
	       HostPrecedenceViolation=314,
	       [AliasAttribute("precedence-cutoff")]
	       PrecedenceCutoff=315,
	    [AliasAttribute("source-quench")]
	    SourceQuench=4,
	    [AliasAttribute("redirect")]
	    Redirect=5,
	       [AliasAttribute("network-redirect")]
	       NetworkRedirect=500,
	       [AliasAttribute("host-redirect")]
	       HostRedirect=501,
	       [AliasAttribute("TosNetworkRedirect")]
	       TosNetworkRedirect=502,
	       [AliasAttribute("TosHostRedirect")]
           TosHostRedirect=503,
        [AliasAttribute("echo-request", "ping")]
        EchoRequest=8,
        [AliasAttribute("router-advertisement")]
        RouterAdvertisement=9,
        [AliasAttribute("router-solicitation")]
        RouterSolicitation=10,
        [AliasAttribute("time-exceeded","ttl-exceeded")]
        TimeExceeded=11,
           [AliasAttribute("ttl-zero-during-transit")]
           TtlZeroDuringTransit=1100,
           [AliasAttribute("ttl-zero-during-reassembly")]
           TtlZeroDuringReassembly=1101,
        [AliasAttribute("parameter-problem")]
        ParameterProblem=12,
           [AliasAttribute("ip-header-bad")]
           IpHeaderBad=1202,
           [AliasAttribute("required-option-missing")]
           RequiredOptionMissing=1201,
        [AliasAttribute("timestamp-request")]
        TimestampRequest=13,
        [AliasAttribute("timestamp-reply")]
        TimestampReply=14,
        [AliasAttribute("AddressMaskRequest")]
        AddressMaskRequest=17,
        [AliasAttribute("AddressMaskReply")]
        AddressMaskReply=18,
		/// <summary>
		/// Default non-usable value. This value is for initialization
		/// purposes and must not be used.
		/// </summary>
	    [AliasAttribute("any")]
	    Any=ushort.MaxValue
	}	
}
