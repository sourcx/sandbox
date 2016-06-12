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

nil_super_thing = SuperThing.new(nil)

if nil_super_thing
  puts 'nil_super_thing is true'
else
  puts 'nil_super_thing is a lie'
end

if nil
  puts 'nil is true'
else
  puts 'nil is a lie'
end

puts 'well ok then...'
puts (nil_super_thing == nil)
puts (nil == nil_super_thing)
