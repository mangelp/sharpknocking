#!/bin/sh
# This test only works if it is run from the current directory and every
# configuration file needed is in the directory.

EXECPATH=" ../ManagedSource/KnockingDaemon/bin/Debug/KnockingDaemon.exe"
ARGS=" $2 $3 $4 $5 $6 $7 $8"

if [ $# -ge 1 -a "$1" = "-d" ]; then
	# We run detached from console and protected from being killed easily
	nohup mono $EXECPATH $ARGS </dev/null &
elif [ $# -eq 1 -a "$1" = "-h" ] ; then
    echo "$0 [-d,--dbg,--verb,--noruledaemon,--nocapturedaemon]"
    echo "  -d: Runs the sharp knocking daemon detached from console."
    echo "      If it is not specified is run from console."
	echo "  --dbg: Activates debuggin mode. Use in conjuntion with -v, -vv, -vvv"
	echo "  -v, -vv, -vvv: If debuggin mode is activated print more information"
	echo "  --noruledaemon: Don't init the rule daemon"
	echo "  --nocapturedaemon: Don't init the capture daemon"
	echo "  --ldcurrent: Load current rule set from iptables"
else
	#run from console
	mono $EXECPATH $ARGS $1
fi

