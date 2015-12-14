# export TINY_KEY, AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY
require 'tinify'

Tinify.key = ENV['TINY_KEY']

source = Tinify.from_file("images/in/small.jpg")

puts "Doing normal compression"
source.to_file("images/out/small-compressed.jpg")

# puts "Resizing image"
resized = source.resize(method: 'fit', width: 100, height: 80)
resized.to_file("images/out/small-compressed-resized.jpg")

aws_access_key_id = ENV['AWS_ACCESS_KEY_ID']
aws_secret_access_key = ENV['AWS_SECRET_ACCESS_KEY']

source.store(
  service: 's3',
  aws_access_key_id: aws_access_key_id,
  aws_secret_access_key: aws_secret_access_key,
  path: 'tinypng-com-preview/testing/small.jpg'
)
resized.store(
  service: 's3',
  aws_access_key_id: aws_access_key_id,
  aws_secret_access_key: aws_secret_access_key,
  path: 'tinypng-com-preview/testing/small-resized.jpg'
)

puts "\nThank you come again!"
puts "Compression count: #{Tinify.compression_count}"
