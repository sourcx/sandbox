/*
 * Function with external names explicitly defined same as local.
 * Makes no sense to do this since they are the same.
 */
func addTogether_1(one one: Int, other other: Int) -> Int {
    return one + other
}

print(addTogether_1(one: 1, other: 2))


/*
 * Function with external names same as local.
 */
func addTogether_2(one: Int, other: Int) -> Int {
    return one + other
}

print(addTogether_2(one: 1, other: 2))


/*
 * Function with external names disabled.
 */
func addTogether_3(_ one: Int, _ other: Int) -> Int {
    return one + other
}

print(addTogether_3(3, 4))

// This is not possible
// print(addTogether_3(one: 3, other: 4))


/*
 * Storing the function in a variable removes the external parameter names.
 */
func addTogether_4(oneExternal one: Int, otherExternal other: Int) -> Int {
    return one + other
}

var functionRef = addTogether_4
print(functionRef(5, 6))


/*
 * Create the same function without func keyword.
 * Do this with a closure. Note that the ':' sign is used to define a type in Swift.
 */
var addTogetherClosure_1: (Int, Int) -> Int = { (one, other) in
    return one + other
}

print(addTogetherClosure_1(7, 8))

// Korter
var addTogetherClosure_2: (Int, Int) -> Int = {
    return $0 + $1
}

print(addTogetherClosure_2(9, 10))

// Mand
var addTogetherClosure_3: (Int, Int) -> Int = {
    $0 + $1
}

print(addTogetherClosure_3(11, 12))

