require 'faraday'
require 'typhoeus/adapters/faraday'

class HttpClient
  class << self
    def client(options)
      Thread.current[self.to_s] ||= new(options)
    end

    def client=(new_client)
      Thread.current[self.to_s] = new_client
    end
  end

  def initialize(options)
    @url = options[:url]
    @timeout = options[:timeout] ? options[:timeout] : nil
    @adapter = options[:adapter] ? options[:adapter].to_sym : :typhoeus
  end

  def get
    res = client.get(@url)
  end

  protected

  def client
    @client ||= Faraday.new(url: @url) do |conn|
      conn.options.open_timeout = @timeout
      conn.options.timeout = @timeout
      conn.adapter(@adapter)
    end
  end
end
