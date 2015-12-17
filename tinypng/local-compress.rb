# export TINY_KEY=<key_here>
require "net/https"
require "uri"
require "time"
require 'json'

puts "Starting compression... #{ENV["TINY_KEY"]}"
uri = URI.parse("http://api.tinypng.local:8001/shrink")
http = Net::HTTP.new(uri.host, uri.port)
request = Net::HTTP::Post.new(uri.request_uri)
request.basic_auth("api", ENV["TINY_KEY"])

# OPTION 1
# puts "POST file contents of images/in/small.jpg to #{uri}."
# body = File.binread("images/in/small.jpg")
# response = http.request(request, body)

# OPTION 2
puts "POST url in JSON body to #{uri}."
request.content_type = "application/json"
body = {}
# body = { source: { url: "https://tinypng.com/images/jpg/panda-png-happy.png" } }
response = http.request(request, body.to_json)

puts "-" * 100
puts response
puts "-" * 100

unless response.code == "201"
  puts "Problem: #{response.body}"
  raise "Compression failed"
end

puts "Compressed image located at: #{response['location']}"

puts "Getting compressed image"
compressed = http.get(response["location"]).body

puts "Write compressed file to images/out/small.jpg"
File.binwrite("images/out/small.jpg", compressed)

puts "\nThank you come again!"
puts "Compression count: #{response['Compression-Count']}"
