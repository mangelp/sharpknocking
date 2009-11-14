// Definitions.cs
//
//  Copyright (C) 2007 iSharpKnocking project
//  Created by mangelp<@>gmail[*]com
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

using Developer.Common.Types;

namespace IptablesSharp.Core.Extensions
{
	
	/// <summary>
    /// Match extensions. The only supported are: tcp, udp, icmp and state.
    /// </summary>
    /// <remarks>
    /// Each match extension handling must be developed in a separate
    /// class.
    /// </remarks>
    public enum MatchExtensions
    {
        /// <summary>
        /// Default value. None selected.
        /// </summary>
        None=0,
        /// <summary>
        /// Non included extension specified. Customized extension.
        /// </summary>
        CustomExtension,
        /// <summary>
        /// Account traffic for all hosts in defined network/netmask
        /// </summary>
        [Alias("account")]
        Account,
        /// <summary>
        /// Matches packets based on their address type.
        /// </summary>
        [Alias("addrtype")]
        Addrtype,
        /// <summary>
        /// Matches the SPIs in Authentication header of OPsec packets
        /// </summary>
        [Alias("ah")]
        Ah,
        /// <summary>
        /// Matches on whether the packet is part of a master connection or one
        /// of its children.
        /// </summary>
        [Alias("childlevel")]
        Childlevel,
        /// <summary>
        /// Add a comment to the rule.
        /// </summary>
        [Alias("comment")]
        Comment,
        /// <summary>
        /// This matches if a specific /proc filename is '0' or '1'
        /// </summary>
        [Alias("condition")]
        Condition,
        /// <summary>
        /// Match by how many bytes or packets a connection have transfered so
        /// far, or by average bytes per packet.
        /// </summary>
        [Alias("connbytes")]
        Connbytes,
        /// <summary>
        /// Allows you to restrict the number of parallel TCP connections to a
        /// server per client IP address
        /// </summary>
        [Alias("connlimit")]
        Connlimit,
		/// <summary>
		/// 
		/// </summary>
        [Alias("connmark")]
        Connmark,
		/// <summary>
		/// 
		/// </summary>
        [Alias("conrate")]
        Connrate,
		/// <summary>
		/// 
		/// </summary>
        [Alias("conntrack")]
        Conntrack,
		/// <summary>
		/// 
		/// </summary>
        [Alias("dccp")]
        Dccp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("dscp")]
        Dscp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("dstlimit")]
        Dstlimit,
		/// <summary>
		/// 
		/// </summary>
        [Alias("ecn")]
        Ecn,
		/// <summary>
		/// 
		/// </summary>
        [Alias("esp")]
        Esp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("fuzzy")]
        Fuzzy,
		/// <summary>
		/// 
		/// </summary>
        [Alias("hashlimit")]
        Hashlimit,
		/// <summary>
		/// 
		/// </summary>
        [Alias("helper")]
        Helper,
        /// <summary>
        /// Icmp packet type matching
        /// </summary>
        [Alias("icmp")]
        Icmp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("iprange")]
        Iprange,
		/// <summary>
		/// 
		/// </summary>
        [Alias("ipv4options")]
        Ipv4options,
		/// <summary>
		/// 
		/// </summary>
        [Alias("length")]
        Length,
		/// <summary>
		/// 
		/// </summary>
        [Alias("limit")]
        Limit,
		/// <summary>
		/// 
		/// </summary>
        [Alias("mac")]
        Mac,
		/// <summary>
		/// 
		/// </summary>
        [Alias("mark")]
        Mark,
		/// <summary>
		/// 
		/// </summary>
        [Alias("mport")]
        Mport,
		/// <summary>
		/// 
		/// </summary>
        [Alias("multiport")]
        Multiport,
		/// <summary>
		/// 
		/// </summary>
        [Alias("nth")]
        Nth,
		/// <summary>
		/// 
		/// </summary>
        [Alias("osf")]
        Osf,
		/// <summary>
		/// 
		/// </summary>
        [Alias("owner")]
        Owner,
		/// <summary>
		/// 
		/// </summary>
        [Alias("physdev")]
        Physdev,
		/// <summary>
		/// 
		/// </summary>
        [Alias("pkttype")]
        Pkttype,
		/// <summary>
		/// 
		/// </summary>
        [Alias("policy")]
        Policy,
		/// <summary>
		/// 
		/// </summary>
        [Alias("psd")]
        Psd,
		/// <summary>
		/// 
		/// </summary>
        [Alias("quota")]
        Quota,
		/// <summary>
		/// 
		/// </summary>
        [Alias("random")]
        Random,
		/// <summary>
		/// 
		/// </summary>
        [Alias("realm")]
        Realm,
		/// <summary>
		/// 
		/// </summary>
        [Alias("recent")]
        Recent,
		/// <summary>
		/// 
		/// </summary>
        [Alias("sctp")]
        Sctp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("set")]
        Set,
		/// <summary>
		/// 
		/// </summary>
        [Alias("state")]
        State,
		/// <summary>
		/// 
		/// </summary>
        [Alias("string")]
        String,
        /// <summary>
        /// Tcp protocol extensions
        /// </summary>
        [Alias("tcp")]
        Tcp,
		/// <summary>
		/// 
		/// </summary>
        [Alias("tcpmss")]
        Tcpmss,
		/// <summary>
		/// 
		/// </summary>
        [Alias("time")]
        Time,
		/// <summary>
		/// 
		/// </summary>
        [Alias("tos")]
        Tos,
		/// <summary>
		/// 
		/// </summary>
        [Alias("ttl")]
        Ttl,
		/// <summary>
		/// 
		/// </summary>
        [Alias("u32")]
        U32,
        /// <summary>
        /// Udp protocol extensions
        /// </summary>
        [Alias("udp")]
        Udp,
        /// <summary>
        /// Makes some random sanity checks in the packets. This must have
        /// bugs so don't use it.
        /// </summary>
        [Alias("unclean")]
        Unclean
    }
    
    /// <summary>
    /// Target extensions for the -j (--jump) option of the iptables command-line
    /// </summary>
    /// <remarks>
    /// Not all the extensions have been implemented. At the time of this writing
    /// there were plans on implementing ulog, log and reject targets.
	/// </remarks>
    public enum TargetExtensions
    {
        [Alias("BALANCE")]
        Balance,
        [Alias("CLASSIFY")]
        Classify,
        [Alias("CLUSTERIP")]
        ClusterIP,
        [Alias("CONNMARK")]
        Connmark,
        [Alias("DNAT")]
        Dnat,
        [Alias("DSCP")]
        Dscp,
        [Alias("ECN")]
        Ecn,
        [Alias("IPMARK")]
        IPMark,
        [Alias("IPV4OPTSSTRIP")]
        IPv4Optsstrip,
        [Alias("LOG")]
        Log,
        [Alias("MARK")]
        Mark,
        [Alias("MASQUERADE")]
        Masquerade,
        [Alias("MIRROR")]
        Mirror,
        [Alias("NETMAP")]
        Netmap,
        [Alias("NFQUEUE")]
        NfQueue,
        [Alias("NOTRACK")]
        Notrack,
        [Alias("REDIRECT")]
        Redirect,
        [Alias("REJECT")]
        Reject,
        [Alias("ROUTER")]
        Route,
        [Alias("SAME")]
        Same,
        [Alias("SET")]
        Set,
        [Alias("SNAT")]
        Snat,
        [Alias("TARIP")]
        TarIP,
        [Alias("TCPMSS")]
        TcpMss,
        [Alias("TOS")]
        Tos,
        [Alias("TRACE")]
        Trace,
        [Alias("TTL")]
        Ttl,
        [Alias("ULOG")]
        Ulog,
        [Alias("XOR")]
        Xor
    }
}
