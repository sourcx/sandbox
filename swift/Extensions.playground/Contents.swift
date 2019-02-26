// Create a Protocol that requires to implement the talk() function
protocol TalkProtocol {
    mutating func talk()
}

// Separate class
class Person {
    let name = "John"
}

// Conform the class to the Protocol
extension Person: TalkProtocol {
    func talk() {
        print("Hi I am \(name).")
    }
}

var dude = Person()
dude.talk()

class AnotherPerson: TalkProtocol {
    func talk() {}
}

struct NotAPerson: TalkProtocol {
    var thingy: String

    // Mutating is needed to change properties in the struct
    // Therefore in protocol it should also be mutating
    mutating func talk() {
        thingy = "thingy"
        print("No")
    }
}
