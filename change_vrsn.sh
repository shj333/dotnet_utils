#!/bin/bash

if [ $# -ne 1 ] ; then
  echo "Usage: change_vrsn.sh [new vrsn]"
  exit
fi

cur_year=`date +'%Y'`

find . -name AssemblyInfo.cs -exec sed -i "s/[[:digit:]]\+\.[[:digit:]]\+\.[[:digit:]]\+\.[[:digit:]]\+/$1/" {} \; -exec sed -i "s/\"Copyright.*[[:digit:]]\+/\"Copyright 2012-$cur_year/" {} \;

