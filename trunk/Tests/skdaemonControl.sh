#!/bin/sh
# This test only works if it is run from the current directory and every
# configuration file needed is in the directory.

EXECPATH=" ../SharpKnocking/KnockingDaemonControl/bin/Debug/KnockingDaemonControl.exe"
ARGS=""

mono $EXECPATH $ARGS

