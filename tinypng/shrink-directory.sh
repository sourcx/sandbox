#!/bin/bash
# export TINY_KEY=<key_here>
echo "shrinking images/in/*, place results in images/out/"
mkdir -p tiny

for file in images/in/*
do
  if [[ -f $file ]]; then
    echo "compressing $file"
    url=`curl --user "api:$TINY_KEY" --data-binary @$file https://api.tinypng.com/shrink -D- -o /dev/null -s | grep Location | cut -c11-`
    wget -q $url -o "images/out/`basename $file`"
  fi
done
