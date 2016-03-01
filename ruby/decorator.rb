# http://ruby-doc.org/stdlib-2.2.1/libdoc/delegate/rdoc/SimpleDelegator.html

require 'delegate'

class Thing
  def name
    "frank"
  end
end

class SuperThing < SimpleDelegator
  def name
    "super #{super}"
  end
end

t = Thing.new
puts t.name

t = SuperThing.new(t)
puts t.name
