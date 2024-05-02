using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDBSample;

class Program
{
    static void Main(string[] args)
    {
        // MongoDB connection string
        // mongodb+srv://
        string connectionString = File.ReadAllText("connectionString.txt");

        // Database name
        string databaseName = "sampledb";

        // Collection name
        string collectionName = "samplecollection";

        // Creating a MongoClient to connect to MongoDB
        var client = new MongoClient(connectionString);

        // Accessing the database
        var database = client.GetDatabase(databaseName);

        // Accessing the collection
        var collection = database.GetCollection<Blob>(collectionName);
        // Seed(collection);
        Search(collection);
    }

    private static void Seed(IMongoCollection<Blob> collection)
    {
        var staticTags = new List<Tag>
        {
            new() { Key = "Tag1", Value = "ValueA" },
            new() { Key = "Tag1", Value = "ValueB" },
            new() { Key = "Tag1", Value = "ValueC" },
            new() { Key = "Tag2", Value = "ValueA" },
            new() { Key = "Tag2", Value = "ValueB" },
            new() { Key = "Tag2", Value = "ValueC" },
            new() { Key = "Tag3", Value = "Value3" },
            new() { Key = "Tag4", Value = "Value4" },
            new() { Key = "Tag5", Value = "Value5" }
        }.ToArray();

        var blobs = new List<Blob>();

        // for (int i = 0; i < 1000000; i++) // Insert 1 million blobs
        for (int i = 1000000; i < 99000000; i++) // Insert 99 million blobs. 33 863 000
        {
            var blob = new Blob
            {
                Id = Guid.NewGuid(),
                FileName = $"SampleFile{i}.txt",
                FullPath = $"2023/01/01/SampleFile{i}.txt",
                Size = 1024,
                tags = [
                    staticTags[i % staticTags.Length],
                    staticTags[(i + 1) % staticTags.Length],
                    staticTags[(i + 2) % staticTags.Length],
                ]
            };

            blobs.Add(blob);

            if (blobs.Count == 1000)
            {
                collection.InsertMany(blobs);
                blobs.Clear();
                Console.WriteLine($"Inserted {i + 1} blobs");
            }
        }
    }

    private static void Search(IMongoCollection<Blob> collection)
    {
        var tags = new List<Tag>
        {
            new() { Key = "Tag1", Value = "ValueA" },
            new() { Key = "Tag2", Value = "ValueB" }
        };

        // Build filter based on provided tags
        var filterBuilder = Builders<Blob>.Filter;
        var filters = tags.Select(tag => filterBuilder.Eq("tags.Key", tag.Key) & filterBuilder.Eq("tags.Value", tag.Value));
        var filter = filterBuilder.And(filters);

        var sort = Builders<Blob>.Sort.Descending(blob => blob.FileName);

        Console.WriteLine($"Searching for blobs with tags: {string.Join(", ", tags.Select(tag => $"{tag.Key}={tag.Value}"))}");
        var matchingBlobs = collection.Find(filter).Sort(sort).Limit(10);
        Console.WriteLine($"Found {matchingBlobs.CountDocuments()} blobs matching the search criteria");

        var matchingBlobsList = matchingBlobs.ToList();
        foreach (var blob in matchingBlobsList)
        {
            Console.WriteLine($"Blob with Id: {blob.Id} matches the search criteria");
            Console.WriteLine(blob.ToBsonDocument());
            Console.WriteLine("");
        }
    }
}
