# http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=E29D62CE72B4D4069083E39AE2E76000&steamids=76561198019000000 # https://developer.valvesoftware.com/wiki/Steam_Web_API#GetPlayerSummaries_.28v0002.29

require 'open-uri'
require 'nokogiri'
require 'json'

class Awesomenaut
  APPNAME = "204300"
  FRIENDS = { 76561198019000000 => "name",
              76561197960811111 => "name2" }

  LEAGUE_20 = 483354
  ME_PLAYER = 76561198019000000
  # from this you can get all leaderboard ids
  ALL_LEADERBOARD_URL = "http://steamcommunity.com/stats/#{APPNAME}/leaderboards/?xml=1"
  LEADERBOARD_URL = "http://steamcommunity.com/stats/204300/leaderboards/#{LEAGUE_20}?xml=1&steamid=#{ME_PLAYER}"
  PLAYER_SUMMARIES_URL = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=E29D62CE72B4D4069083E39AE2E76000&steamids="

  class << self
    def update_em!
      update_leaderboard_info!
      update_player_summaries!
    end

    def update_leaderboard_info!
      # puts "Opening XML from #{LEADERBOARD_URL}"
      # doc = Nokogiri::XML(open(LEsADERBOARD_URL))
      doc = File.open("leaderboard-info.xml") { |f| Nokogiri::XML(f) }
      doc.xpath("//response/entries/entry").each do |entry|
        puts entry.inspect
        # naut = Awesomenaut.where(steamid: doc.xpath("steamid").first)
        puts entry.xpath("score").first.text
        puts entry.xpath("rank").first.text
        puts entry.xpath("details").first.text
        # puts doc.xpath("score").first.text
        # puts naut.inspect
        # naut.score = doc.xpath("score").first.text
        # naut.rank = doc.xpath("rank").first.text
        # naut.details = doc.xpath("details").first.text
        # naut.save!
      end
    end

    def update_player_summaries!
      # puts "Opening JSON from " + PLAYER_SUMMARIES_URL + FRIENDS.keys.join(",")
      # profile_info = JSON.load(open(PLAYER_SUMMARIES_URL + FRIENDS.keys.join(",")))
      profile_info = JSON.load(File.read("profile-info.json"))
      puts profile_info
      profile_info["response"]["players"].each do |player|

      end
    end
  end
end

Awesomenaut.update_em!
