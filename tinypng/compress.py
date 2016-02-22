import os, tinify

tinify.key = os.getenv('TINY_KEY')
print tinify.key

tinify.from_file('images/in/small.png').to_file('images/out/small-compressed.png')
print "Compression Done! Compression done this month: {0}".format(tinify.compression_count)

tiny = tinify.from_url('https://raw.githubusercontent.com/tinify/tinify-ruby/master/test/examples/voormedia.png')
tiny.to_file('images/out/small-compressed.png')
print "Compression Done! Compression done this month: {0}".format(tinify.compression_count)
