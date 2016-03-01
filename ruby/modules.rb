module PropertyA
end

class Thing
  module PropertyB
  end

  include PropertyA
  include PropertyB
end

t = Thing.new

puts t.class.included_modules
