# structs
require 'oj'
require 'json'

class World
  module Things
    Struct.new("Person", :name)
  end

  include Things

  def show
    jan = Struct::Person.new("Jan")
    puts jan.name
  end
end

World.new().show
jaap = Struct::Person.new("Jaap")
puts jaap.name

# Object, Hash, Array, String, Fixnum, Float, true, false, or nil.
# Not working with struct
begin
  puts Oj.dump(jaap, mode: :strict)
rescue TypeError => e
  puts e
end

class Person
  attr_accessor :name

  def initialize(name)
    self.name = name
  end

  def to_h
    { "aa" => "bb" }
  end
end

kees = Person.new("Kees")
puts kees.name

puts "Json dump"
puts JSON.dump(kees).inspect

begin
  puts Oj.dump(kees, mode: :strict)
rescue TypeError => e
  puts e
end

