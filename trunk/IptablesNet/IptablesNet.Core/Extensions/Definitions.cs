
using System;

using Developer.Common.Types;

namespace IptablesNet.Core.Extensions
{
	
	/// <summary>
    /// Match extensions. The only supported are: tcp, udp, icmp and state.
    /// </summary>
    /// <remarsk>
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
        /// <summary>
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
        [Alias("connmark")]
        Connmark,
        [Alias("conrate")]
        Connrate,
        [Alias("conntrack")]
        Conntrack,
        [Alias("dccp")]
        Dccp,
        [Alias("dscp")]
        Dscp,
        [Alias("dstlimit")]
        Dstlimit,
        [Alias("ecn")]
        Ecn,
        [Alias("esp")]
        Esp,
        [Alias("fuzzy")]
        Fuzzy,
        [Alias("hashlimit")]
        Hashlimit,
        [Alias("helper")]
        Helper,
        /// <summary>
        /// Icmp packet type matching
        /// </summary>
        [Alias("icmp")]
        Icmp,
        [Alias("iprange")]
        Iprange,
        [Alias("ipv4options")]
        Ipv4options,
        [Alias("length")]
        Length,
        [Alias("limit")]
        Limit,
        [Alias("mac")]
        Mac,
        [Alias("mark")]
        Mark,
        [Alias("mport")]
        Mport,
        [Alias("multiport")]
        Multiport,
        [Alias("nth")]
        Nth,
        [Alias("osf")]
        Osf,
        [Alias("owner")]
        Owner,
        [Alias("physdev")]
        Physdev,
        [Alias("pkttype")]
        Pkttype,
        [Alias("policy")]
        Policy,
        [Alias("psd")]
        Psd,
        [Alias("quota")]
        Quota,
        [Alias("random")]
        Random,
        [Alias("realm")]
        Realm,
        [Alias("recent")]
        Recent,
        [Alias("sctp")]
        Sctp,
        [Alias("set")]
        Set,
        [Alias("state")]
        State,
        [Alias("string")]
        String,
        /// <summary>
        /// Tcp protocol extensions
        /// </summary>
        [Alias("tcp")]
        Tcp,
        [Alias("tcpmss")]
        Tcpmss,
        [Alias("time")]
        Time,
        [Alias("tos")]
        Tos,
        [Alias("ttl")]
        Ttl,
        [Alias("u32")]
        U32,
        /// <summary>
        /// Udp protocol extensions
        /// </summary>
        [Alias("udp")]
        Udp,
        [Alias("unclean")]
        /// <summary>
        /// Makes some random sanity checks in the packets. This must have
        /// bugs so don't use it.
        /// </summary>
        Unclean
    }
    
    /// <summary>
    /// Target extensions for the -j (--jump) option of the iptables command-line
    /// </summary>
    /// <remarks>
    /// Not all the extensions have been implemented. At the time of this writing
    /// there were plans on implementing ulog, log and reject targets.
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
