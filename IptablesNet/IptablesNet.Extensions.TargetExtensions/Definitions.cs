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

namespace IptablesNet.Extensions.Targets
{
    public enum BalanceTargetOptions
    {
        [Alias("to-destination")]
        ToDestination    
    }
    
    public enum ClassifyTargetOptions
    {
        [Alias("set-class")]
        SetClass
    }
    
    public enum ClusterIPHashingMode
    {
        [Alias("sourceip")]
        SourceIP,
        [Alias("sourceip-sourceport")]
        SourceIPSourcePort,
        [Alias("sourceip-sourceport-destport")]
        SourceIPSourcePortDestport
    }
    
    public enum ClusterIPTargetOptions
    {
        [Alias("new")]
        New,
        [Alias("hashmode")]
        Hashmode,
        [Alias("clustermac")]
        Clustermac,
        [Alias("total-nodes")]
        TotalNodes,
        [Alias("local-node")]
        LocalNode,
        [Alias("hash-init")]
        HashInit
    }
    
    public enum ConnmarkTargetOptions
    {
        [Alias("set-mark")]
        SetMark,
        [Alias("save-mark")]
        SaveMark,
        [Alias("restore-mark")]
        RestoreMark,
        [Alias("mask")]
        Mask
    }
    
    public enum DnatTargetOptions
    {
        [Alias("to-destination")]
        ToDestination    
    }
    
    public enum DscpTargetOptions
    {
        [Alias("set-dscp")]
        SetDscp,
        [Alias("set-dscp-class")]
        SetDscpClass
    }
    
    public enum EcnTargetOptions
    {
        [Alias("ecn-tcp-remove")]
        EcnTcpRemove
    }
    
    public enum IpMarkTargetOptions
    {
        [Alias("addr")]
        Addr,
        [Alias("and-mask")]
        AndMask,
        [Alias("or-mask")]
        OrMask
    }
    
    public enum IPv4OptsstripTargetOptions
    {}
    
    public enum LogTargetOptions
    {
        [Alias("log-level")]
        LogLevel,
        [Alias("log-prefix")]
        LogPrefix,
        [Alias("log-tcp-sequence")]
        LogTcpSequence,
        [Alias("log-tcp-options")]
        LogTcpOptions,
        [Alias("log-in-options")]
        LogIpOptions,
        [Alias("log-uid")]
        LogUid
    }
    
    public enum MarkTargetOptions
    {
        [Alias("set-mark")]
        SetMark
    }
    
    public enum MasqueradeTargetOptions
    {
        [Alias("to-ports")]
        ToPorts
    }
    
    public enum MirrorTargetOptions
    {
    }
    
    public enum NetmapTargetOptions
    {
        [Alias("to")]
        To    
    }
    
    public enum NfQueueTargetOptions
    {
        [Alias("queue-num")]
        QueueNum
    }
    
    public enum RedirectTargetOptions
    {
        [Alias("to-ports")]
        ToPorts
    }
    
    public enum RejectTargetOptions
    {
        [Alias("reject-with")]
        RejectWith
    }
    
    public enum RejectIcmpTypes
    {
        [Alias("icmp-net-unreachable")]
        IcmpNetUnreachable,
        [Alias("icmp-host-unreachable")]
        IcmpHostUnreachable,
        [Alias("icmp-port-unreachable")]
        IcmpPortUnreachable,
        [Alias("icmp-proto-unreachable")]
        IcmpProtoUnreachable,
        [Alias("icmp-net-prohibited")]
        IcmpNetProhibited,
        [Alias("icmp-host-prohibited")]
        IcmpHostProhibited,
        [Alias("icmp-admin-prohibited")]
        IcmpAdminProhibited
    }
    
    public enum RouteTargetOptions
    {
        
        [Alias("oif")]
        Oif,
        [Alias("iif")]
        Iif,
        [Alias("gw")]
        Gw,
        [Alias("continue")]
        Continue,
        [Alias("tee")]
        Tee
    }
    
    
    public enum SameTargetOptions
    {
        [Alias("to")]
        To,
        [Alias("nodst")]
        Nodst
    }
    
    public enum SetTargetOptions
    {
        [Alias("add-set")]
        AddSet,
        [Alias("del-set")]
        DelSet
    }
    
    public enum SnatTargetOptions
    {
        [Alias("to-source")]
        ToSource    
    }
    
    public enum TarpitTargetOptions
    {}
    
    public enum TcpmssTargetOptions
    {
        [Alias("set-mss")]
        SetMss,
        [Alias("clamp-mss-to-pmtu")]
        ClampMssToPmtu
    }
    
    public enum TosTargetOptions
    {
        [Alias("set-tos")]
        SetTos
    }
    
    public enum TraceTargetOptions
    {}
    
    public enum TtlTargetOptions
    {
        [Alias("ttl-set")]
        TtlSet,
        [Alias("ttl-dec")]
        TtlDec,
        [Alias("ttl-inc")]
        TtlInc
    }
    
    public enum UlogTargetOptions
    {
        [Alias("ulog-nlgroup")]
        UlogNlgroup,
        [Alias("ulog-prefix")]
        UlogPrefix,
        [Alias("ulog-cprange")]
        UlogCprange,
        [Alias("ulog-qthreshold")]
        UlogQthreshold
    }
    
    public enum XorTargetOptions
    {
        [Alias("key")]
        Key,
        [Alias("block-size")]
        BlockSize
    }
}
