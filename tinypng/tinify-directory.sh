#!/bin/bash
# export TINY_KEY=<key_here>
echo "shrinking in/*, place results in out/"
mkdir -p out

for file in in/*
do
  if [[ -f $file ]]; then
    echo "compressing $file"
    url=`curl --user "api:$TINY_KEY" --data-binary @"$file" https://api.tinypng.com/shrink -D- -o /dev/null -s | grep -i Location`
    echo "got url: $url"
    url=`echo $url | cut -c11- | dos2unix`
    echo "got url: $url"
    out="out/`basename \"$file\"`"
    echo "downloading $url saving to $out"
    wget --quiet $url --output-file $out
  fi
done
