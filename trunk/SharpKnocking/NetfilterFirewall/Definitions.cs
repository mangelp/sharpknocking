// created on 14/01/2007 at 14:23

// Definitions for iptables

using System;
using SharpKnocking.Common;

namespace SharpKnocking.NetfilterFirewall
{

    
    /// <summary>
    /// Built in chains in iptables. Not all the builtin chains are applicable
    /// to every table. Each table contains a subset of builtin chains.
    /// </summary>
    public enum BuiltInChains:short
    {
        //If set to this must be a user defined chain.
        None=0,
        //Incoming packets chain
        Input,
        //Outcoming packets chain
        Output,
        //??
        Forward,
        //??
        Prerouting,
        //??
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
        /// The target is a custom extension. This option is unsupported.
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
    public enum PacketTables
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
 
