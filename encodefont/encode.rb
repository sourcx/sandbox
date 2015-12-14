require 'base64'

# file = File.binread("./din-medium.ttf")
# enc = Base64.encode64(file)
# puts enc

Base64.encode64(file.open('/assets/abadi_mt_condensed_light_regular-webfont.ttf') { |io| io.read })
