
using System;
using System.ComponentModel;

using Developer.Common.Types;

namespace IptablesNet.Extensions.Match
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
	    //The packet has created a new connection or is from a connection which
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
	    //No need to None 
	    [AliasAttribute("icmp-type")]
	    IcmpType
	}
	
	public enum StateMatchOptions
	{
	    //No need to None 
	    [AliasAttribute("state")]
	    State
	}
	
	public enum UdpMatchOptions
	{
	    //Default value. Not-valid for iptables but usefull.
	    None=0,
	    [AliasAttribute("source-port","sport")]
	    SourcePort,
	    [AliasAttribute("destination-port","dport")]
	    DestinationPort
	}
	
	public enum TcpMatchOptions
	{
	    //Default value. Not-valid for iptables but usefull.
	    None=0,
	    [AliasAttribute("source-port","sport")]
	    SourcePort,
	    [AliasAttribute("destination-port","dport")]
	    DestinationPort,
	    [AliasAttribute("tcp-flags")]
	    TcpFlags,
	    [AliasAttribute("syn")]
	    Syn,
	    [AliasAttribute("tcp-option")]
	    TcpOption
	    //This option is in the man page but not shown with the command
	    // iptables -m tcp -h
	    // v. 1.3.5 of iptables
        //[AliasAttribute("mss")]
        //Mss
	}
	
}
