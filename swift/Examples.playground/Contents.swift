/*
 * Computer properties.
 *
 */
var storage = ["computed": 42]

var computed: Int {
    get {
        return storage["computed"]!
    }
    set(val) {
        storage["computed"] = val
    }
}

print(computed)
computed = 100
print(computed)

// Don't use get if it is a read-only property.
var computedReadOnly: Int {
    return 555
}

print(computedReadOnly)

// error: cannot assign to value: 'computedReadOnly' is a get-only property
// computedReadOnly = 666


/*
 * Properties of a struct
 */
var globalThing = 300

struct Settings {
    static let length = 0

    // Runs and set first time it is called.
    static let globalThingFunction = {
        return globalThing
    }()

    static let amountOfThings = {
        return 200
    }()
}

print("Settings.length: \(Settings.length)")
print("Settings.amountOfThings: \(Settings.amountOfThings)")
globalThing = 500
print("Settings.globalThingFunction \(Settings.globalThingFunction)")
globalThing = 1500
print("Settings.globalThingFunction \(Settings.globalThingFunction)")


struct Structuur {
    let a: String
    var c: String
    static let d: Int = 42

    var berekend: String {
        return "leuk " + a
    }

    init(a: String) {
        self.a = a
        self.c = "Doei"
    }
}

var s = Structuur(a: "Hoi")

print(s.berekend)
