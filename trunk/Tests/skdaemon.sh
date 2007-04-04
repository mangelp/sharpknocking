#!/bin/sh
# This test only works if it is run from the current directory and every
# configuration file needed is in the directory.

EXECPATH=" ../ManagedSource/KnockingDaemon/bin/Debug/KnockingDaemon.exe"
ARGS=" $2 $3 $4 $5 $6 $7 $8"

if [ $# -ge 1 -a "$1" = "-d" ]; then
	# We run detached from console and protected from being killed easily
	echo "Running daemon"
	nohup mono $EXECPATH $ARGS </dev/null &
else
	#run from console
	echo "Running daemon attached to console"
	mono $EXECPATH $ARGS $1
fi

