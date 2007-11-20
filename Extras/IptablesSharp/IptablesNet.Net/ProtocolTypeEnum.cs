
using System;
using SharpKnocking.Common;

namespace IptablesNet.Net
{
	
    /// <summary>
    /// Types of protocols. Extracted from /etc/protocols.
    /// See http://www.iana.org/assignments/protocol-numbers 
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// hop-by-hop options for ipv6
        /// </summary>
        [Alias("hopopt")]
        Hopopt=0,
        /// <summary>
        /// internet control message protocol
        /// </summary>
        [Alias("icmp")]
        Icmp=1, 
        /// <summary>
        /// internet group management protocol
        /// </summary>
        [Alias("igmp")]
        Igmp=2, 
        /// <summary>
        /// gateway-gateway protocol
        /// </summary>
        [Alias("ggp")]
        Ggp=3, 
        /// <summary>
        /// IP encapsulated in IP (officially "IP")
        /// </summary>
        [Alias("ipencap")]
        IpEncap=4, 
        /// <summary>
        /// ST datagram mode
        /// </summary>
        [Alias("st")]
        St =5, 
        /// <summary>
        /// transmission control protocol
        /// </summary>
        [Alias("tcp")]
        Tcp=6, 
        /// <summary>
        /// CBT, Tony Ballardie <A.Ballardie@cs.ucl.ac.uk>
        /// </summary>
        [Alias("cbt")]
        Cbt=7, 
        /// <summary>
        /// exterior gateway protocol
        /// </summary>
        [Alias("egp")]
        Egp=8, 
        /// <summary>
        /// any private interior gateway (Cisco: for IGRP)
        /// </summary>
        [Alias("igp")]
        Igp=9, 
        /// <summary>
        /// BBN RCC Monitoring
        /// </summary>
        [Alias("bbn-rcc")]
        BbnRcc=10, 
        /// <summary>
        /// Network Voice Protocol
        /// </summary>
        [Alias("nvp")]
        Nvp=11, 
        /// <summary>
        /// PARC universal packet protocol
        /// </summary>
        [Alias("pup")]
        Pup=12, 
        /// <summary>
        /// ARGUS
        /// </summary>
        [Alias("argus")]
        Argus=13, 
        /// <summary>
        /// EMCON
        /// </summary>
        [Alias("emcon")]
        Emcon=14, 
        /// <summary>
        /// Cross Net Debugger
        /// </summary>
        [Alias("xnet")]
        Xnet=15, 
        /// <summary>
        /// Chaos
        /// </summary>
        [Alias("chaos")]
        Chaos=16, 
        /// <summary>
        /// user datagram protocol
        /// </summary>
        [Alias("udp")]
        Udp=17, 
        /// <summary>
        /// Multiplexing protocol
        /// </summary>
        [Alias("mux")]
        Mux=18,
        /// <summary>
        /// DCN Measurement Subsystems
        /// </summary>
        [Alias("dcn")]
        Dcn=19,
        /// <summary>
        /// Host monitoring protocol
        /// </summary>
        [Alias("hmp")]
        Hmp=20,
        /// <summary>
        /// Packet radio measurement protocol
        /// </summary>
        [Alias("prm")]
        Prm=21,
        /// <summary>
        /// Xerox NS IDP
        /// </summary>
        [Alias("xns-idp")]
        XnsIdp=22, 
        /// <summary>
        /// Trunk-1
        /// </summary>
        [Alias("trunk-1")]
        Trunk1=23,
        /// <summary>
        /// Trunk-2
        /// </summary>
        [Alias("trunk-2")]
        Trunk2=24, 
        /// <summary>
        /// Leaf-1
        /// </summary>
        [Alias("leaf-1")]
        Leaf1=25,
        /// <summary>
        /// Leaf-2
        /// </summary>
        [Alias("leaf-2")]
        Leaf2=26, 
        /// <summary>
        /// "reliable datagram" protocol
        /// </summary>
        [Alias("rdp")]
        Rdp=27,
        /// <summary>
        /// Internet Reliable Transaction Protocol
        /// </summary>
        [Alias("irtp")]
        Irtp=28,
        /// <summary>
        /// ISO Transport Protocol Class 4
        /// </summary>
        [Alias("iso-tp4")]
        IsoTp4=29,
        /// <summary>
        /// Bulk Data Transfer Protocol
        /// </summary>
        [Alias("netblt")]
        Netblt=30,
        /// <summary>
        /// MFE Network Services Protocol
        /// </summary>
        [Alias("mfe-nsp")]
        MfeNsp=31,
        /// <summary>
        /// MERIT Internodal Protocol
        /// </summary>
        [Alias("merit-inp")]
        MeritInp=32,
        /// <summary>
        /// Datagram Congestion Control Protocol
        /// </summary>
        [Alias("dccp")]
        Dccp=33, 
        /// <summary>
        /// Third Party Connect Protocol
        /// </summary>
        [Alias("3pc")]
        ThirdPc=34, 
        /// <summary>
        /// Inter-Domain Policy Routing Protocol
        /// </summary>
        [Alias("idpr")]
        Idpr=35,
        /// <summary>
        /// Xpress Tranfer Protocol
        /// </summary>
        [Alias("xtp")]
        Xtp=36,
        /// <summary>
        /// Datagram Delivery Protocol
        /// </summary>
        [Alias("ddp")]
        Ddp=37,
        /// <summary>
        /// IDPR Control Message Transport Proto
        /// </summary>
        [Alias("idpr-cmtp")]
        IdprCmtp=38,
        /// <summary>
        /// TP++ Transport Protocol
        /// </summary>
        [Alias("tp++")]
        TpPP=39,
        /// <summary>
        /// IL Transport Protocol
        /// </summary>
        [Alias("il")]
        Il=40,
        /// <summary>
        /// Ipv6
        /// </summary>
        [Alias("ipv6")]
        IpV6=41,
        /// <summary>
        /// Source Demand Routing Protocol
        /// </summary>
        [Alias("sdrp")]
        Sdrp=42 ,
        /// <summary>
        /// Routing header for ipv6
        /// </summary>
        [Alias("ipv6-route")]
        IPv6Route=43,
        /// <summary>
        /// Fragment header for ipv6
        /// </summary>
        [Alias("ipv6-frag")]
        IPv6Frag=44,
        /// <summary>
        /// Inter-domain routing protocol
        /// </summary>
        [Alias("idrp")]
        Idrp=45,
        /// <summary>
        /// Reservation protocol
        /// </summary>
        [Alias("rsvp")]
        Rsvp=46,
        /// <summary>
        /// General Routing Encapsulation
        /// </summary>
        [Alias("gre")]
        Gre=47,    
        /// <summary>
        /// Dynamic Source Routing Protocol
        /// </summary>
        [Alias("dsr")]
        Dsr=48,
        /// <summary>
        /// BNA
        /// </summary>
        [Alias("bna")]
        Bna=49,
        /// <summary>
        /// Encap Security Payload
        /// </summary>
        [Alias("esp")]
        Esp=50,   
        /// <summary>
        /// Autentication Header
        /// </summary>
        [Alias("ah")]
        Ah=51,   
        /// <summary>
        /// Integrated Net Layer Security TUBA
        /// </summary>
        [Alias("I-nlsp")]
        INlsp=52,   
        /// <summary>
        /// IP with Encryption
        /// </summary>
        [Alias("swipe")]
        Swipe=53,   
        /// <summary>
        /// NBMA Address Resolution Protocol
        /// </summary>
        [Alias("narp")]
        Narp=54,   
        /// <summary>
        /// IP Mobility
        /// </summary>
        [Alias("mobile")]
        Mobile=55,   
        /// <summary>
        /// Transport Layer Security Protocol using Kryptonet key management
        /// </summary>
        [Alias("tlsp")]
        Tlsp=56,
        /// <summary>
        /// SKIP
        /// </summary>
        [Alias("skip")]
        Skip=57,   
        /// <summary>
        /// ICMP for IPv6
        /// </summary>
        [Alias("ipv6-icmp")]
        IPv6Icmp=58,
        /// <summary>
        /// No Next Header for IPv6
        /// </summary>
        [Alias("ipv6-nonxt")]
        IPv6NoNxt=59,
        /// <summary>
        /// Destination options for IPv6
        /// </summary>
        [Alias("ipv6-opts")]
        IPv6Opts=60,
//        /// <summary>
//        /// Any host internal protocol
//        /// </summary>
//        [Alias("")]
//        =61,   
        /// <summary>
        /// General Routing Encapsulation
        /// </summary>
        [Alias("cftp")]
        Cftp=62,   
//        /// <summary>
//        /// Any local network
//        /// </summary>
//        [Alias("")]
//        =63,   
        /// <summary>
        /// SATNET and Backroom EXPAK
        /// </summary>
        [Alias("SatExpak")]
        SatExpak=64,
        /// <summary>
        /// Kryptolan
        /// </summary>
        [Alias("kryptolan")]
        Kryptolan=65,        
        /// <summary>
        /// MIT Remote Virtual Disk Protocol
        /// </summary>
        [Alias("rvd")]
        Rvd=66,
        /// <summary>
        /// Internet Pluribus Packet Core
        /// </summary>
        [Alias("ippc")]
        Ippc=67,
//        /// <summary>
//        /// Any distributed file system
//        /// </summary>
//        [Alias("")]
//        =68,
        /// <summary>
        /// SATNET Monitoring
        /// </summary>
        [Alias("sat-mon")]
        SatMon=69,
        /// <summary>
        /// Visa Protocol
        /// </summary>
        [Alias("Visa")]
        Visa=70,
        /// <summary>
        /// Internet Packet Core Utility
        /// </summary>
        [Alias("ipcv")]
        Ipcv=71,
        /// <summary>
        /// Computer Protocol Network Executive
        /// </summary>
        [Alias("cpnx")]
        Cpnx=72,
        /// <summary>
        /// Computer Protocol Heart Beat
        /// </summary>
        [Alias("cphb")]
        Cphb=73,
        /// <summary>
        /// Wang span network
        /// </summary>
        [Alias("wsn")]
        Wsn=74,
        /// <summary>
        /// Packet Video Protocol
        /// </summary>
        [Alias("pvp")]
        Pvp=75,
        /// <summary>
        /// Backroom SATNET Monitoring
        /// </summary>
        [Alias("br-sat-mon")]
        BrSatMon=76,
        /// <summary>
        /// SUN ND PROTOCOL- Temporary
        /// </summary>
        [Alias("sun-nd")]
        SunNd=77,
        /// <summary>
        /// WIDEBAND Monitoring
        /// </summary>
        [Alias("wb-mon")]
        WbMon=78,
        /// <summary>
        /// WIDEBAND EXPAK
        /// </summary>
        [Alias("WbExpak")]
        WbExpak=79,
        /// <summary>
        /// ISO Internet Protocol
        /// </summary>
        [Alias("iso-ip")]
        IsoIp=80,
        /// <summary>
        /// VMTP
        /// </summary>
        [Alias("vmtp")]
        Vmtp=81,
        /// <summary>
        /// SECURE-VMTP
        /// </summary>
        [Alias("secure-vmtp")]
        SecureVmtp=82,
        /// <summary>
        /// VINES
        /// </summary>
        [Alias("vines")]
        Vines=83,
        /// <summary>
        /// TTP
        /// </summary>
        [Alias("ttp")]
        Ttp=85,
        /// <summary>
        /// NSFNET-IGP
        /// </summary>
        [Alias("nsfnet-igp")]
        NsfnetIgp=85,
        /// <summary>
        /// Dissimilar Gateway Protocol
        /// </summary>
        [Alias("dgp")]
        Dgp=86,
        /// <summary>
        /// TCF
        /// </summary>
        [Alias("tcf")]
        Tcf=87,
        /// <summary>
        /// EIGRP
        /// </summary>
        [Alias("eigrp")]
        Eigrp=88,
        /// <summary>
        /// OSPFIGP
        /// </summary>
        [Alias("ospfigp")]
        Ospfigp=89,
        /// <summary>
        /// Sprite RPC Protocol
        /// </summary>
        [Alias("sprite-rpc")]
        SpriteRpc=90,
        /// <summary>
        /// Locus Address Resolution Protocol
        /// </summary>
        [Alias("larp")]
        Larp=91, 
        /// <summary>
        /// Multicast Transport Protocol
        /// </summary>
        [Alias("mtp")]
        Mtp=92,
        /// <summary>
        /// AX.25 Frames
        /// </summary>
        [Alias("ax.25")]
        Ax25=93,
        /// <summary>
        /// IP-within-IP Encapsulation Protocol
        /// </summary>
        [Alias("ipip")]
        Ipip=94,         
        /// <summary>
        /// Mobile Internetworking Control Pro.
        /// </summary>
        [Alias("miscp")]
        Miscp=95,
        /// <summary>
        /// Semaphore Communications Sec. Pro.
        /// </summary>
        [Alias("scc-sp")]
        SccSp=96,
        /// <summary>
        /// Ethernet-within-IP Encapsulation
        /// </summary>
        [Alias("etherip")]
        etherip=97,     
        /// <summary>
        /// Encapsulation Header
        /// </summary>
        [Alias("Encap")]
        Encap=98,
//        /// <summary>
//        /// Any private encryption scheme
//        /// </summary>
//        [Alias("")]
//        =99,
        /// <summary>
        /// GMTP
        /// </summary>
        [Alias("gmtp")]
        Gmtp=100,
        /// <summary>
        /// Ipsilon Flow Management Protocol
        /// </summary>
        [Alias("Ifmp")]
        Ifmp=101,
        /// <summary>
        /// PNNI over IP
        /// </summary>
        [Alias("pnni")]
        Pnni=102,
        /// <summary>
        /// Protocol Independent Multicast
        /// </summary>
        [Alias("pim")]
        Pim=103,
        /// <summary>
        /// ARIS
        /// </summary>
        [Alias("aris")]
        Aris=104,
        /// <summary>
        /// SCPS
        /// </summary>
        [Alias("scps")]
        Scps=105,
        /// <summary>
        /// QNX
        /// </summary>
        [Alias("qnx")]
        Qnx=106,
        /// <summary>
        /// Active Networks
        /// </summary>
        [Alias("a/n")]
        AN=107,
        /// <summary>
        /// IP Payload Compression Protocol
        /// </summary>
        [Alias("ipcomp")]
        IPComp=108,
        /// <summary>
        /// Sitara Networks Protocol
        /// </summary>
        [Alias("vines")]
        Snp=109,
        /// <summary>
        /// Compaq Peer Protocol
        /// </summary>
        [Alias("compaq-peer")]
        CompaqPeer=110,
        /// <summary>
        /// IPX in IP
        /// </summary>
        [Alias("ipx-in-ip")]
        IpxInIp=111,
        /// <summary>
        /// Virtual Router Redundancy Protocol
        /// </summary>
        [Alias("vrrp")]
        Vrrp=112,
        /// <summary>
        /// PGM Reliable Transport Protocol
        /// </summary>
        [Alias("pgm")]
        Pgm=113,
//        /// <summary>
//        /// Any 0-hop protocol
//        /// </summary>
//        [Alias("")]
//        =114,
        /// <summary>
        /// Layer Two Tunneling Protocol
        /// </summary>
        [Alias("l2tp")]
        L2Tp=115,
        /// <summary>
        /// D-II Data Exchange
        /// </summary>
        [Alias("ddx")]
        Ddx=116,
        /// <summary>
        /// Interactive Agent Transfer Protocol
        /// </summary>
        [Alias("iatp")]
        Iatp=117,
        /// <summary>
        /// Schedule Transfer Protocol
        /// </summary>
        [Alias("stp")]
        Stp=118,
        /// <summary>
        /// SpectraLing Radio Protocol
        /// </summary>
        [Alias("srp")]
        Srp=119,
        /// <summary>
        /// UTI
        /// </summary>
        [Alias("uti")]
        Uti=120,
        /// <summary>
        /// Simple Message Protocol
        /// </summary>
        [Alias("smp")]
        Smp=121,	
        /// <summary>
        /// SM
        /// </summary>
        [Alias("sm")]
        Sm=122,	
        /// <summary>
        /// Performance Transparency Protocol
        /// </summary>
        [Alias("ptp")]
        Ptp=123,
        /// <summary>
        /// ISIS over IPv4
        /// </summary>
        [Alias("isis")]
        Isis=124,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("fire")]
        Fire=125,	
        /// <summary>
        /// Combat Radio Transport Protocol
        /// </summary>
        [Alias("crtp")]
        Crtp=126,	
        /// <summary>
        /// Combat Radio User Datagram
        /// </summary>
        [Alias("crudp")]
        Crudp=127,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("sscopmce")]
        Sscopmce=128,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("iplt")]
        Iplt=129,	
        /// <summary>
        /// Secure Packet Shield
        /// </summary>
        [Alias("sps")]
        Sps=130,
        /// <summary>
        /// Private IP Encapsulation within IP
        /// </summary>
        [Alias("pipe")]
        Pipe=131,	
        /// <summary>
        /// Stream Control Transmission Protocol
        /// </summary>
        [Alias("sctp")]
        Sctp=132,	
        /// <summary>
        /// Fibre Channel
        /// </summary>
        [Alias("fc")]
        Fc=133,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("rsvp-e2e-ignore")]
        RsvpE2eIgnore=134,
//        /// <summary>
//        /// Mobility Header
//        /// </summary>
//        [Alias("")]
//        =135,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("udplite")]
        UdpLite=136,	
        /// <summary>
        /// 
        /// </summary>
        [Alias("mpls-in-ip")]
        MplsInIp=137
//   138-252 Unassigned                                       [IANA]
//   253     Use for experimentation and testing           [RFC3692] 
//   254     Use for experimentation and testing           [RFC3692] 
//   255                 Reserved                             [IANA]
     }
}
