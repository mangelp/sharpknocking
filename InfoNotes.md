## Dropping udp packets and efficiency ##

Taken from the netfilter mail list:

> Hello,

> msn a écrit :

> > So, it is NOT possible to decreasing the CPU usage of the massive UDP

> > handling in the linux kernel ? any comments would be nice to me.

> You cannot decrease the packet processing load in the network interface
> driver, unless you use a more "efficient" network card or driver. As
> Martijn wrote, the best you can do at iptables level is drop the packets
> as soon as possible, in the PREROUTING chain of the 'raw' table even
> though this is not the primary purpose of the 'raw' table. This will
> save CPU usage for the traversal of subsequent iptables chains, the
> connection tracking if it is enabled (as opposed to filtering in the
> PREROUTING chain of the 'mangle' table), the input routing decision and
> the fragment reassembly (as opposed to filtering in the INPUT chain of
> any table).

> The path is as follows :

> interface -> raw/PREROUTING -> conntrack & fragment reassembly ->
> mangle/PREROUTING -> nat/PREROUTING -> routing decision -> fragment
> reassembly -> mangle/INPUT -> filter/INPUT -> local delivery