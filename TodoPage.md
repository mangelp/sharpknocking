## Full pending feature list for next versions ##

### KnockingDaemon ###
  * Protect only a range of ports and let the rest open to the net and/or redirect them to an existing chain in the firewall.
  * Allow more that one port to open after a successfull call sequence.
  * Specify actions in the sequences. The actions may be executed by the daemon when a successfull call is detected and it specifies an action.
  * Avoid duplicate sequences from an already authorized ip (for a port(s)).
  * Add a timer to delete authentications (rules in iptables that allows an ip to open a communication for a set of ports) in the daemon.
  * Create script to start the daemon at init.
  * Usage statistics.
  * Improve knocking security.
  * Improve daemon design. There is a severe security hole in remoting.
  * Add a daemon configuration file.
  * Command line options to configure things.
  * Remote control program to query daemon information and do start/stop operations.
  * When doing a restart in the daemon all rules must be removed.
  * There is a [wrapper](http://www.chrishowie.com/)([svn repo](https://layla.chrishowie.com/svn/pcap-sharp)) for libpcap done in c#. Can we get any benefits of using it instead of a tcpdump process?
  * Add a script to the init.d so this can be integrated within the rest of services in the machine.
  * The packet capture method (we use a process with tcpdump) can be improved, we should refactor things like this...
  * It looks like that this piece of the suite needs more a big refactoring that nothing.

### PortKnocker ###
  * Add support to check if the last authentication if still valid.
  * Winforms version to use it on windows and make it more portable for default windows setups (that doesn't have a gtk+mono setup). We should also split the baked from the ui.
  * Prepare a bundle of required libraries including the minimal set of mono binaries required to run the app from another system. Today usb memories are cheaper than ever and a 15-30 MB bundle should fit without problems.

### Doorman ###
  * ~~Interactive mode from Doorman~~.
  * The admin should be able to shutdown/start interactive mode at will and to configure if it is started by default.
  * Remote administration mode.
  * Improve security measures.
  * Netfiler rules editor. A default rule can be configured to be used when the daemon shut downs or to redirect packets from a host that got a valid access.
  * When creating sequences with a random number of port show an option to select only those ports over 1024.
  * The configuration for the method of knocking is going to get increased and we should need a new configuration dialog with all the options.
  * There should be an "expert" mode and a "don't bother me with strange things" mode.

### SharpKnocking.NetfilterFirewall ###
  * ~~This class library needs tons of love. Needs refactoring and deep testing~~.
  * ~~It's a big thing that have little use for SharpKnocking. Maybe it should be removed and replaced the functionality that provides with simpler stuff and make it an independent library~~.
  * This library is being refactorized in the subproject IptablesNet and in a future it will be replaced with the result of that.

### IptablesNet ###
  * Be aware this is going to be called IptablesSharp soon ...

### Misc ###
  * Review some files: ~~README, INSTALL~~, TODO, ...
  * To improve security we must think about encription, pdu format, authentication process, protocols allowed, actions allowed, bugs, exploits, ...
  * We need a roadmap!
  * Ipv6 support, now it only works throught ipv4.
  * Design documentation.