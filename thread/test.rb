require './http_client'

http_client = HttpClient.new(url: 'http://frankevers.nl')
puts http_client.get.body
