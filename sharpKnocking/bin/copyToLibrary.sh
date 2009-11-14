#!/bin/sh

#updates the libraries only if are newer
echo "Copying libraries from $1 to $2. Updating enabled"
if [ ! -d $1 ] ; then
	echo "$1 is not a valid directory. Exiting"
	exit 1
fi

d=`dirname $2`
if [ ! -d $d ] ; then
	mkdir -p $d
fi
cp -u $1/*.dll $2/
