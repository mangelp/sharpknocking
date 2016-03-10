# Packet capture options #

We would like to have more than one packet capture method so we are able to compare those methods or swith between them at will.

### 1- Tcpdum. ###
We still have the prototype version of this listening method, but once we get some updates into the knocking sequence we must either update or remove it.

### 2- Libpcap. ###
There is a managed libpacp library wrapper called [libpcap-sharp](http://www.chrishowie.com/pcap-sharp/) that seems to work. We should try it.

### 3- Iptables targets ###
There are some ipteble's targets that logs packets from the firewall so we can work with them in user-space:

**3.1- ULOG:**
It can multicast the packets to userspace throught a netlink socket. The userspace processes can subscribe and receive the packets. This is how our daemon can get the packets and analize it.
This target has the advantage that we can control the packets logged with rules so we can reduce the global amount of packets that the daemon has to process and then have a better performance.
There is a userspace tool called ulogd to log the packets to a text file or a database.

**3.2- NFQUEUE:**
This requires a newer kernel and is an extension of the QUEUE target. Allows to put the packets to a queue identified by a 16-bit number. This looks like the same that ULOG but the options of the target are different.
I think that once we have the ULOG target we should try this too. But we need to develop the userspace tool to read the packets directly from netlink socket; maybe we will need a glue c library that does the work and a c# wrapper.
There is also a utility library to help when working with netlink sockets called libnl that is included in the distros (fc6 comes with 1.0pre5) and that can simplify the task.

**3.3- QUEUE:**
Don't ask me, but i vaguely remember have read something about that this was deprecated due that with NFQUEUE we have support for ipv6 (and ipv4) and with QUEUE we only have ipv4.



# Knocking sequence #

Currently, the knocking sequence is a set of random udp ports in the target machine with the number of knock sequence in the payload of the packet with no encryption.
We must set privacy policies for packet sending to make them difficult to read and make the knocking attempt difficult to detect.

There are some ideas for this:

1- Send only one packet with the data encrypted (can use public key from the server).

2- Add more packet types to send: UDP, TCP, ICMP.

3- Introduce "dummy" packets in the sequence description aleatory, to be replaced by a random number of packets with random payloads which will be sent to the server. This will cause the sequence to look more random.

4- Detect replay attempts. The sequences will be used more than one time, but each time we need to check something to see differences with the previous one or anyone capturing traffic will access the system.
Maybe we should ban sequences, but that converts it in a good way of doing DOS attacks to users.

5- Annonymization of sender ip. But this is how we identify who is sending a knocking sequence.

6- Each time a user completes a knocking sequence a session must be started so all the traffic from that user will get into the server through authorized ports, but after a time of inactivity the session must be deleted.

7- Send packets through annonymous proxies. This needs additional information in the packet to know who is sending it.

8- Detect brute force attacks and ban attackers.

9- Give sequences and users and id that will identify them and maybe a pin code to add a little more security.

10- The user can sign the data in the knocks with his private key so the server can check his identity (it must have the public key for the user, of course).

11- We can require from the user a random rumber to identify the knocking attemp, that number should be sent with his user id and the sequence id. When the knocking sequence starts to come in the packets will be sorted by that number so we can track how many connection attemps is doing a user.

12- The data can be encrypted and signed in the client and then sent in pieces along with the knocking sequence. Then we must have a mechanism to know what packets have real data and the order to assemble the pieces to get the data back (once unencrypted and checked the sign).

13- Use one-time passwords.

# Other implementations #

We must look seriously at the existing implementations.

**1- Advanced PortKnocking Suite**
This Suite uses TCP packets to send data and commands to the server. It encrypts data using DES.
  * Year: 2004
  * Language: Perl
  * [WEB](http://www.iv2-technologies.com/~rbidou/)
  * [README](http://www.iv2-technologies.com/~rbidou/apk.README.html)
  * [DOWNLOAD](http://www.iv2-technologies.com/~rbidou/apk-1.0.tar.gz)

**2- Cryptknock**
Uses an ecrypted string to send the knock sequence instead of sending a sequence of udp or tcp packets.

From the sourceforge page:
> _(...)Encryption of the knock string is performed with RC4 using a secret key
> derived from a Diffie-Hellman key agreement.  The entire process takes
> 3 UDP packets.(...)_
  * Year: 2004
  * Language: C
  * [WEB](http://cryptknock.sourceforge.net/)
  * [README](http://cryptknock.sourceforge.net/README.txt)
  * [DOWNLOAD](http://cryptknock.sourceforge.net/cryptknock-1.0.1.tar.gz)

**3- The Doorman**
Protects services running behind tcp ports:

> _(...)This particular implementation deviates a bit from his original proposal, in
> that the doorman watches for only a single UDP packet.   To get the doorman to open
> up, the packet must contain an MD5 hash which correctly hashes a shared secret,
> salted with a 32-bit random number, the identifying user or group-name, and the
> requested service port-number.(...)_
  * Year: 2005
  * Language: perl
  * [WEB](http://doorman.sourceforge.net/)
  * [MANPAGE of daemon](http://doorman.sourceforge.net/doormand_8.html)
  * [MANPAGE of client](http://doorman.sourceforge.net/knock_1.html)
  * [DOWNLOAD](http://sourceforge.net/project/showfiles.php?group_id=92394&release_id=257407)

**4- fwknop**
Uses a single packet to authenticate the knocking but it relies in a more modern concept called Single Port Authorization(SPA).

  * Year: 2007 (Still alive)
  * Language: Perl
  * [WEB](http://www.cipherdyne.org/fwknop/)
  * [DOC](http://www.cipherdyne.org/fwknop/docs/)
  * [DOWNLOAD](http://www.cipherdyne.org/fwknop/download/)

Related interesting things:
  * ["Linux Firewalls: Attack Detection and Response"](http://www.cipherdyne.org/blog/2006/11/book-announcement-linux-firewalls-attack-detection-and-response.html)
  * [Paper about SPA](http://www.cipherdyne.org/fwknop/docs/SPA.html)