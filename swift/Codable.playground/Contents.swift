import Foundation

/*
 * Codable
 *
 * https://developer.apple.com/documentation/foundation/archives_and_serialization/encoding_and_decoding_custom_types
 *
 */

// Simple scenario
struct Landmark: Codable {
    var name: String
    var foundingYear: Int
}

var landmark = Landmark(name: "landmark", foundingYear: 1900)
print(landmark)

var encodedLandmark = try! JSONEncoder().encode(landmark)
print(String(data: encodedLandmark, encoding: .utf8))

var decodedLandmark = try! JSONDecoder().decode(Landmark.self, from: encodedLandmark)
print(decodedLandmark)


// Using CodingKeys
struct OtherLandmark: Codable {
    var name: String
    var foundingYear: Int
    var thing: String

    enum CodingKeys: String, CodingKey {
        case name = "n"
        case foundingYear = "f"
        case thing
    }
}

var otherLandmark = OtherLandmark(name: "landmark", foundingYear: 1901, thing: "thing")
print(otherLandmark)

var encodedOtherLandmark = try! JSONEncoder().encode(otherLandmark)
print(String(data: encodedOtherLandmark, encoding: .utf8))

var decodedOtherLandmark = try! JSONDecoder().decode(OtherLandmark.self, from: encodedOtherLandmark)
print(decodedOtherLandmark)


// With a property that is also Codable
enum RequestType: Int, Codable {
    case login = 0
    case register = 1
}

struct Request: Codable {
    let username: String?
    let password: String?
    let type: RequestType

    enum CodingKeys: String, CodingKey {
        case username = "u"
        case password = "p"
        case type = "t"
    }
}

let request = Request(username: "username", password: "password", type: .login)
print(request)

let encodedRequest = try! JSONEncoder().encode(request)
print(String(data: encodedRequest, encoding: .utf8))

let decodedRequest = try! JSONDecoder().decode(Request.self, from: encodedRequest)
print(decodedRequest)

// Property list instead of JSON
let encodedRequestPLIST = try! PropertyListEncoder().encode(request)

let decodedRequestPLIST = try! PropertyListDecoder().decode(Request.self, from: encodedRequestPLIST)
print(decodedRequestPLIST)
