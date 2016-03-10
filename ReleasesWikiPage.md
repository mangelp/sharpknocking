## Initial release 0.1 ##

This morning (UTC+2) we have released the initial version of SharpKnocking suite. It is basically functional but there is a lot of work to do.

It can be installed after compiling the sources. To compile and run you need a sane install of mono and the related gtk libraries. We have developed it with mono 1.2.3 and requires gtk-sharp 2.10 to build. It may work with older versions of mono like 1.2.2.1 but the last versions fixes a lot of things so we recommed you to get updated frequently.

Our development enviroment is monodevelop 0.13 so you can find the solution file (.mds) under the directory SharpKnocking.

**Compile**

# ./autogen.sh --prefix=/usr/local
make

**Install**

# sudo make install

**Features**

With this initial version you can:

  * Close all open ports and give access to certain ips.
  * Manage the list of sequences in the server-side and export them to use with the PortKnocker app.
  * Use the PortKnocker app to call to a remote sistem to open a port.

**Bug reports**

This version is feature-incomplete and must contain bugs. Please report them [here](http://code.google.com/p/sharpknocking/issues/list).


