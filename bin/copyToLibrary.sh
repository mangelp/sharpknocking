#!/bin/sh

#updates the libraries only if are newer
echo "Copying libraries from $1 to $2. Updating enabled"
cp -u $1/*.dll $2/
