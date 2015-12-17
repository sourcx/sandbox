# export TINY_KEY, AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY
require 'tinify'

Tinify.key = ENV['TINY_KEY']

puts "Correct URL and image"
source = Tinify.from_url("https://s3-eu-west-1.amazonaws.com/tinypng-com-preview/testing/source.png")
puts source.inspect
puts

puts "URL to html page"
begin
  source = Tinify.from_url("http://frankevers.nl")
  puts source.inspect
rescue Tinify::ClientError => e
  puts e
end
puts

puts "Unresolveable http URL"
begin
  source = Tinify.from_url("http://jfadskfjfljkfldas")
  puts source.inspect
rescue Tinify::ClientError => e
  puts e
end
puts

puts "URL wrong protocol"
begin
  source = Tinify.from_url("stuff://jfadskfjfljkfldas")
  puts source.inspect
rescue Tinify::ClientError => e
  puts e
end
puts


# puts "Doing normal compression"
# source.to_file("images/out/small-compressed.jpg")
#
# puts "Resizing image"
# resized = source.resize(
#   method: 'fit',
#   width: 100,
#   height: 80
# )
# resized.to_file("images/out/small-compressed-resized.jpg")
#
# aws_access_key_id = ENV['AWS_ACCESS_KEY_ID']
# aws_secret_access_key = ENV['AWS_SECRET_ACCESS_KEY']
#
# source.store(
#   service: 's3',
#   aws_access_key_id: aws_access_key_id,
#   aws_secret_access_key: aws_secret_access_key,
#   path: 'tinypng-com-preview/testing/small.jpg'
# )
# resized.store(
#   service: 's3',
#   aws_access_key_id: aws_access_key_id,
#   aws_secret_access_key: aws_secret_access_key,
#   path: 'tinypng-com-preview/testing/small-resized.jpg'
# )
#
# puts "\nThank you come again!"
# puts "Compression count: #{Tinify.compression_count}"
