require 'open-uri'
require 'nokogiri'

url = "http://steamcommunity.com/stats/204300/leaderboards"
PLAYERRANKING20 = 483354
url = "#{url}/#{PLAYERRANKING20}?xml=1&steamid=X"

# doc = Nokogiri::XML(open(url))
doc = File.open("test.xml") { |f| Nokogiri::XML(f) }

class Player
  attr_accessor :steamid, :score, :rank, :details

  FRIENDS = { }

  def initialize(doc)
    @steamid = FRIENDS[doc.xpath("steamid").first.text.to_i]
    @score = doc.xpath("score").first.text
    @rank = doc.xpath("rank").first.text
    @details = doc.xpath("details").first.text
  end
end

doc.xpath("//response/entries/entry").each do |entry|
  player = Player.new(entry)
  puts player.steamid
  puts "score: #{player.score}"
  puts "rank: #{player.rank}"
  puts "data: #{player.details}"
  puts
end
