using Amazon.SecurityToken.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBSample;

class Blob
{
    [BsonId]
    public Guid Id { get; set; }

    public string FileName { get; set; }

    public string FullPath { get; set; }

    public int Size { get; set; }

    public List<MongoDBSample.Tag> tags { get; set; }
}
