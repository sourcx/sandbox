#!env ruby
# export TINY_KEY, AWS_ACCESS_KEY_ID, AWS_SECRET_ACCESS_KEY
require 'tinify'

unless ENV['TINY_KEY']
    puts "export TINY_KEY=..."
    exit(42)
end

Tinify.key = ENV['TINY_KEY']

unless ARGV[0]
    puts "usage: tinify-directory.rb [dir]"
    exit(42)
end

puts "Compress all images in #{ARGV[0]} place them in compressed/"
system("mkdir -p compressed")

Dir.new(ARGV[0]).sort.each do |file|
    if file.downcase.include?("jpg") || file.downcase.include?("jpeg") || file.downcase.include?("png")
        infile = "#{ARGV[0]}/#{file}"
        outfile = "compressed/#{file}"
        puts "Compressing #{infile} and saving to #{outfile}"
        Tinify.from_file(infile).to_file(outfile)
    end
end
