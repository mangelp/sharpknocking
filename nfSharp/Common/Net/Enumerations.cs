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

using NFSharp.Common.Types;

namespace NFSharp.Common.Net
{
	//TODO: The enumeration constants of TcpFlags and IcmpTypes should be commented
	//with the meaning of the flag/icmptype.
	
    /// <summary>
    /// Tcp flags
    /// </summary>
	/// <remarks>
	/// This enumeration has the Flag attribute so the values can be combined, all
	/// but the last one. The Any flag has been introduced for initiallization purposses
	/// and should not be used 
	/// </remarks>
    [FlagsAttribute()]
    public enum TcpFlags:byte
    {
        /// <summary>
        /// Default Value. No flags. This value is not real and is only for
        /// initialization. Don't use it directly, methods won't accept it.
        /// </summary>
        None=0,
		/// <summary>
		/// SYN flag
		/// </summary>
        SYN=1,
		/// <summary>
		/// ACK flag
		/// </summary>
        ACK=2,
		/// <summary>
		/// FIN flag
		/// </summary>
        FIN=4,
		/// <summary>
		/// RST flag
		/// </summary>
        RST=8,
		/// <summary>
		/// URG flag
		/// </summary>
        URG=16,
		/// <summary>
		/// PSH flag
		/// </summary>
        PSH=32,
		/// <summary>
		/// All the flags
		/// </summary>
        ALL=63
    }
    
    /// <summary>
    /// Types of icmp
    /// </summary>
    /// <remarks>
    /// The Alias Attribute have been used to provide support for aliases and
    /// searching the original names instead of writing methods in another class
    /// to get the names.
    /// 
    /// Every enumeration member has at least one alias name. The names used
    /// in the commands are the aliased ones
    /// 
    /// In linux you can obtain this list from iptables using this:
    ///      iptables -p icmp -h
    /// 
    /// The type numbers and codes are taken from:
    /// http://www.iana.org/assignments/icmp-parameters
    /// 
    /// The numbers asigned to the constants is the number defined in the standard.
	/// For the subtypes of icmp the number asigned is the result of this calculation:
	///     icmpTypeNumber*100 + icmpSubtypeNumber
    /// </remarks>
	public enum IcmpTypes:ushort
	{
		/// <summary>
		/// Echo reply (pong) type
		/// </summary>
	    [AliasAttribute("echo-reply", "pong")]
	    EchoReply=0,
		/// <summary>
		/// Destination unreachable type
		/// </summary>
	    [AliasAttribute("destination-unreachable")]
	    DestinationUnreachable=3,
			/// <summary>
			/// Network unreachagle subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("network-unreachable")]
	       NetworkUnreachable=300,
			/// <summary>
			/// Host unreachagle subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("host-unreachable")]
	       HostUnreachable=301,
			/// <summary>
			/// Protocol unreachagle subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("protocol-unreachable")]
	       ProtocolUnreachable=302,
			/// <summary>
			/// Port unreachagle subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("port-unreachable")]
	       PortUnreachable=303,
			/// <summary>
			/// Fragmentation needed subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("fragmentation-needed")]
	       FragmentationNeeded=304,
			/// <summary>
			/// Source route failed subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("source-route-failed")]
	       SourceRouteFailed=305,
			/// <summary>
			/// Network unknown subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("network-unknown")]
	       NetworkUnknown=306,
			/// <summary>
			/// Host unknown subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("host-unknown")]
	       HostUnknown=307,
			/// <summary>
			/// Network prohibited subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("network-prohibited")]
	       NetworkProhibited=309,
			/// <summary>
			/// Host prohibited subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("host-prohibited")]
	       HostProhibited=310,
			/// <summary>
			/// TOS network unreachable subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("TOS-network-unreachable")]
	       TosNetworkUnreachable=311,
			/// <summary>
			/// TOS host unreachable subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("TOS-host-unreachable")]
	       TosHostUnreachable=312,
			/// <summary>
			/// Communication prohibited subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("CommunicationProhibited")]
	       Communication_prohibited=313,
			/// <summary>
			/// Host precedence violation subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("host-precedence-violation")]
	       HostPrecedenceViolation=314,
			/// <summary>
			/// Precedence cutoff subtype (destination unreachable)
			/// </summary>
	       [AliasAttribute("precedence-cutoff")]
	       PrecedenceCutoff=315,
		/// <summary>
		/// Source quech type
		/// </summary>
		[AliasAttribute("source-quench")]
	    SourceQuench=4,
		/// <summary>
		/// Redirect type
		/// </summary>
	    [AliasAttribute("redirect")]
	    Redirect=5,
			/// <summary>
			/// Network redirect subtype (redirect)
			/// </summary>
	       [AliasAttribute("network-redirect")]
	       NetworkRedirect=500,
			/// <summary>
			/// Host redirect subtype (redirect)
			/// </summary>
	       [AliasAttribute("host-redirect")]
	       HostRedirect=501,
			/// <summary>
			/// TOS network redirect subtype (redirect)
			/// </summary>
	       [AliasAttribute("TosNetworkRedirect")]
	       TosNetworkRedirect=502,
			/// <summary>
			/// TOS host redirect subtype (redirect)
			/// </summary>
	       [AliasAttribute("TosHostRedirect")]
           TosHostRedirect=503,
		/// <summary>
		/// Echo request (ping) type
		/// </summary>
        [AliasAttribute("echo-request", "ping")]
        EchoRequest=8,
		/// <summary>
		/// Router advertisement type
		/// </summary>
        [AliasAttribute("router-advertisement")]
        RouterAdvertisement=9,
		/// <summary>
		/// Router solicitation type
		/// </summary>
        [AliasAttribute("router-solicitation")]
        RouterSolicitation=10,
		/// <summary>
		/// Time exceeded type
		/// </summary>
        [AliasAttribute("time-exceeded","ttl-exceeded")]
        TimeExceeded=11,
			/// <summary>
			/// TTL zero during transit subtype (time exceeded)
			/// </summary>
           [AliasAttribute("ttl-zero-during-transit")]
           TtlZeroDuringTransit=1100,
			/// <summary>
			/// TTL zero during reassembly subtype (time exceeded)
			/// </summary>
           [AliasAttribute("ttl-zero-during-reassembly")]
           TtlZeroDuringReassembly=1101,
		/// <summary>
		/// Parameter problem type
		/// </summary>
        [AliasAttribute("parameter-problem")]
        ParameterProblem=12,
			/// <summary>
			/// IP header bad subtype (parameter problem)
			/// </summary>
           [AliasAttribute("ip-header-bad")]
           IpHeaderBad=1202,
			/// <summary>
			/// Required option missing subtype (parameter problem)
			/// </summary>
           [AliasAttribute("required-option-missing")]
           RequiredOptionMissing=1201,
		/// <summary>
		/// Timestamp request type
		/// </summary>
        [AliasAttribute("timestamp-request")]
        TimestampRequest=13,
		/// <summary>
		/// Timestamp reply type
		/// </summary>
        [AliasAttribute("timestamp-reply")]
        TimestampReply=14,
		/// <summary>
		/// Address mask request type
		/// </summary>
        [AliasAttribute("AddressMaskRequest")]
        AddressMaskRequest=17,
		/// <summary>
		/// Address mask reply type
		/// </summary>
        [AliasAttribute("AddressMaskReply")]
        AddressMaskReply=18,
		/// <summary>
		/// Default non-usable value. This value is for initialization
		/// purposes and must not be used.
		/// </summary>
	    [AliasAttribute("any")]
	    Any=ushort.MaxValue
	}	
	
	/// <summary>
	/// Types of ip address masks
	/// </summary>
	public enum IPAddressMaskType
	{
		/// <summary>
		/// No valid mask
		/// </summary>
		None = 0,
		/// <summary>
		/// IPV4 mask with the number of bits
		/// </summary>
		Ipv4Short = 1,
		/// <summary>
		/// IPV4 mask with the bit mask
		/// </summary>
		Ipv4Long = 2,
		/// <summary>
		/// IPV6 mask with the number of bits
		/// </summary>
		Ipv6Short = 3,
		/// <summary>
		/// IPV6 mask with the bit mask
		/// </summary>
		Ipv6Long = 4
	}
}
