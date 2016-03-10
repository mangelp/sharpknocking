## What's this? ##

It's a new subproyect that aims to create a managed wrapper to handle rules in the netfilter firewall


## Details ##

Currently the code that is under the SharpKnocking.NetfilterFirewall namespace is being refactorized. That code uses the iptables command-line tool to change the current rule set so currently is a wrapper against that command.

~~In a future it should be better to use some library or API to change the current rule set.~~

It looks like that the better existing api is inside the sources of iptables and is not a valid api to work against it (it can change between revisions). The recommended way to do things is using the commands: iptables, iptables-save, iptables-restore.

A good tip is that the iptables-save can be used to issues commands on demand just writting a COMMIT each time you want the last bunch of command being executed.

## Objetives ##

The main objetive is to provide a library to handle rules with in a object-oriented way. This means that every option and extension (match or target) must have the correspondent implementation object so it can be used with our library.

Another important thing is a good and intuitive UI to mess with the rules easily.

~~The initial version will be uploaded and integrated into svn tree in some weeks.~~
There is now some code in the svn under the directory IptablesNet.