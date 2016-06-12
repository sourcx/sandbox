# pip install tinify
# -*- coding: utf-8 -*-
import os, tinify, sys

tinify.key = os.getenv('TINY_KEY')
print tinify.key

tinify.from_file(u'images/in/你好.jpg').to_file('images/out/small-compressed.png')
print "Compression Done! Compression done this month: {0}".format(tinify.compression_count)

tiny = tinify.from_url('https://raw.githubusercontent.com/tinify/tinify-ruby/master/test/examples/voormedia.png')
tiny.to_file('images/out/small-compressed.png')
print "Compression Done! Compression done this month: {0}".format(tinify.compression_count)

# http://stackoverflow.com/questions/9644099/python-ascii-codec-cant-decode-byte