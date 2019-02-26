import UIKit

func log(_ err: NSError) {
    print("Logger \(err)")
}

enum TestError: Error {
    case unhandledError(String)
    case Base

    var nsError: NSError {
        return NSError(domain: "\(self)", code: 42, userInfo: nil)
    }
}

func gooit() throws {
    let osStuff = "FDJKLFDSJ FKLSDJFDS KJFDSL"
    throw TestError.unhandledError(osStuff)
}

do {
    try gooit()
} catch {
    let castedError = NSError(domain: String(describing: error), code: 42, userInfo: [:])
    log(castedError)
}

//

let ns = NSError.init(domain: "domain", code: 42, userInfo: nil)
print(ns)

let ke = TestError.Base

print(ke is KeynError)
print(ns is KeynError)
print("dude" is NSError)

extension NSError {
    var bla: Bool {
        return true
    }
}

let message = [
    "httpMethod": "hoi",
    "timestamp": "test"
]

func test(message: [String: String]) {
    var message = message
    message["hi"] = "bye"
    print(message)
}
