require "net/https"
require "uri"
require "time"

# export TINY_KEY=<key_here>
def compress(input, output)
  key = ENV["TINY_KEY"]
  uri = URI.parse("https://api.tinify.com/shrink")

  http = Net::HTTP.new(uri.host, uri.port)
  http.use_ssl = true

  # Uncomment below if you have trouble validating our SSL certificate.
  # Download cacert.pem from: http://curl.haxx.se/ca/cacert.pem
  # http.ca_file = File.join(File.dirname(__FILE__), "cacert.pem")

  request = Net::HTTP::Post.new(uri.request_uri)
  request.basic_auth("api", key)

  response = http.request(request, File.binread(input))

  if response.code == "201"
    File.binwrite(output, http.get(response["location"]).body)
  else
    raise "Compression failed"
  end
end

# Dir["./images/*"].each do |name|
#   compress(name, name)
# end

puts "starting compression"
compress("images/small.jpg", "images/out/small.jpg")
puts "compression done"
