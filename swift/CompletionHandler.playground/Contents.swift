/*
 * Completion handlers and stuff
 *
 * https://blog.bobthedeveloper.io/no-fear-closure-in-swift-3-with-bob-part-2-1d79b8c4021d
 */

// Normal closure example
var timesTen: (Int) -> (Int) = { $0 * 10 }
timesTen(42)

func sums(from: Int, until: Int, extraThing: (Int) -> (Int)) -> Int {
    var sums = 0

    for n in from...until {
        sums += extraThing(n)
    }

    return sums
}

sums(from: 1, until: 100, extraThing: timesTen)
sums(from: 1, until: 100, extraThing: { $0 * 10 })


// Trailing closure example
var res = sums(from: 1, until: 100) {
    $0 * 10
}
print(res)


// Completion handlers
// The concept of completion handlers is used for animation and
// notifying users when the download has been completed. e.g.
func downloadStuff(fileName: String, completionHandler: (Bool) -> Void) {
    let res = "success" // failure

    if (res == "success") {
        completionHandler(true)
    } else {
        completionHandler(false)
    }
}

func downloadHandler(res: Bool) {
    if res {
        print("great success")
    }
}

downloadStuff(fileName: "test.png", completionHandler: downloadHandler)
downloadStuff(fileName: "test.gif", completionHandler: { (res: Bool) in
    if res {
        print("super success")
    }
})

// Trailing completion handler
downloadStuff(fileName: "test.jpg") { (res) in
    if res {
        print("epic success")
    }
}

// Shorthand trailing completion handler
downloadStuff(fileName: "test.jpg") {
    if $0 {
        print("epic success?!")
    }
}


// Escaping closure
// If a closure is passed as an argument to a function and it is invoked after the function returns, the closure is escaping.
// It is also said that the closure argument escapes the function body.
var completionHandlers: [() -> Void] = []

// Error
// func functionWithCompletionHandler(block: () -> Void) {
//     completionHandlers.append(block)
// }

// Escaping Closures and works!
func functionWithCompletionHandler2(block: @escaping () -> Void) {
    completionHandlers.append(block)
}


// Test
func request(type: String, path: String?, parameters: [String: String]?, method: String, completionHandler: @escaping ([String: Any]?) -> Void) throws {
    completionHandler(["yes": "no"])
}

try request(type: "", path: "messagePubKey", parameters: ["a": "1"] , method: ".post") { (res) in
    if let json = res {
        print(json)
    }
}
