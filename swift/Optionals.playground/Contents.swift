/*
 * Optionals
 *
 * https://developer.apple.com/documentation/swift/optional
 * https://docs.swift.org/swift-book/LanguageGuide/TheBasics.html
 * https://hackernoon.com/swift-optionals-explained-simply-e109a4297298
 *
 * Optionals are a powerful feature in Swift language which come to solve the problem of non-existing values.
 * You use the Optional type whenever you use optional values, even if you never type the word Optional.
 * Under the hood optional types are merely an enum with two cases.
 */

// Same things

let word1a: String? = nil
let word1b = Optional<String>.none
print(word1a == word1b)

let word2a: String? = "Hi there"
let word2b = Optional<String>.some("Hi there")
print(word2a == word2b)

let num1a: Int? = nil
let num1b = Optional<Int>.none
print(num1a == num1b)

let num2a: Int? = 42
let num2b = Optional<Int>.some(42)
print(num2a == num2b)

print(Optional.none == nil)


/*
 * Optional binding to a variable.
 *
 * Variable only exists within scope.
 * The if let statement unwraps the optional value.
 */
let dictionary = [1: "one", 2: "two"]

if let element = dictionary[1] {
    print("value 1 is \(element)")
} else {
    print("value 1 is not there")
}

if let element = dictionary[3] {
    print("value 3 is \(element)")
} else {
    print("value 3 is not there")
}

var optionalDouble: Double? = 3.14

if let definateDouble = optionalDouble {
    print(definateDouble)
} else {
    print("optionalDouble was nil")
}


/*
 * Optional chaining.
 *
 * To safely access the properties and methods of a wrapped instance,
 * use the postfix optional chaining operator (postfix ?).
 */
if dictionary[2]?.hasSuffix("e") == true {
    print("it has suffix e")
} else {
    print("no suffix e")
}


/*
 * Nil-Coalescing operator ??
 *
 * Use fallback value if expression is nil.
 */
let four = dictionary[4] ?? "the fallback four"
print(four)


/*
 * Unconditional unwrapping for when you know an optional has a value.
 */
var definatelyAValue: Int? = Int("42")
print(definatelyAValue)  // Optional(42)
print(definatelyAValue!) // 42

var notAValue: Int?
// Fatal error: Unexpectedly found nil while unwrapping an Optional value
// print(notAValue!)


/*
 * Forced unwrappig example from hackernoon.com.
 *
 * Under the hood the unwrapping is a switch statement on the variable.
 */
let word: String? = "hi there!"

// Same things
var wordUnwrapped_1 = word!
print(wordUnwrapped_1)

var wordUnwrapped_2: String
switch word {
    case .some(let value):
        wordUnwrapped_2 = value
        print(wordUnwrapped_2)
    case .none:
        print("error!")
        // throw Error
}


/*
 * Implicitly unwrapped optionals.
 *
 * Implicitly unwrapped optionals are useful when an optional’s value is confirmed to
 * exist immediately after the optional is first defined and can definitely be assumed
 * to exist at every point thereafter.
 */
let assumedBool: Bool! = false
print(assumedBool)

let implicitBool: Bool = assumedBool
print(implicitBool)


/*
 * Guard
 *
 * Guard let is similar to if let but the else part of the guard let must exit the
 * current scope, basically calling return. It is used to return early in a function.
 *
 * Below function would end up in a pyramid of doom if we would use 'if let'.
 */
func doStuffAfterChecks(optionalNumber1: Int?, numberAsString2: String?) {
    guard let number1 = optionalNumber1 else {
        print("This is not a valid first number!")
        return
    }
    
    guard let number2 = Int(numberAsString2!) else {
        print("This is not a valid second number!")
        return
    }
    
    print("Succes, we got the numbers: \(number1) \(number2)")
}

let one: Int? = 42
let two: String? = "Maybe"

doStuffAfterChecks(optionalNumber1: Int?(42), numberAsString2: String?("43"))
doStuffAfterChecks(optionalNumber1: one, numberAsString2: two)
