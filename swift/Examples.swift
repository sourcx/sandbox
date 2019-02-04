// Function with external names explicitly defined same as local
func addTogether(one one: Int, other other: Int) -> Int {
    return one + other
}
print(addTogether(one: 1, other: 2))


// Function with external names disabled
func addTogether(_ one: Int, _ other: Int) -> Int {
    return one + other
}
print(addTogether(3, 4))


// Storing the function in a variable removes the external parameter names
func addTogether(one one: Int, other other: Int) -> Int {
    return one + other
}
var functionRef = addTogether
print(functionRef(5, 6))

// Function without func keyword, in the form of closure
var closureRef: (Int, Int) -> Int = { (one, other) in
    return one + other
}
print(closureRef(7, 8))

// Korter
var closureRef: (Int, Int) -> Int = { return $0 + $1 }
print(closureRef(9, 10))

// Korterrrr
var closureRef: (Int, Int) -> Int = { $0 + $1 }
print(closureRef(11, 12))
