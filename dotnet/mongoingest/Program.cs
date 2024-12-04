using System.Diagnostics;
using System.Text.Json;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver.Search;

namespace MongoTest;

public class Program
{
    private static readonly string[] KEYS_TO_IGNORE = ["mtime"];

    public static async Task Main(string[] args)
    {
        string blobSasToken = File.ReadAllText("secret_sas");
        BlobServiceClient blobServiceClient = new(new Uri(blobSasToken));

        string mongoConnectionString = File.ReadAllText("secret_mongodb_richard");
        var mongoClient = new MongoClient(mongoConnectionString);

        // await FillDatabaseWithMetadata(blobServiceClient, mongoClient);

        // await AggregationByMetadataExample(mongoClient);
        // await CountDocuments(mongoClient);
        // await CountDocumentsWithGeometry(mongoClient);
        // await CountDocumentsWithoutGeometry(mongoClient);
        // await CopyGeometryToProperGeometry(mongoClient, offset: 0);
        // await CopyGeometryToEPSG4326(mongoClient);
        // for (int i = 0; i < 10; i++)
        // {
        //     var task1 = SearchByGeo(mongoClient);
        //     var task2 = SearchByGeo(mongoClient);
        //     var task3 = SearchByGeo(mongoClient);
        //     var task4 = SearchByGeo(mongoClient);
        //     var task5 = SearchByGeo(mongoClient);
        //     var task6 = SearchByGeo(mongoClient);
        //     var task7 = SearchByGeo(mongoClient);
        //     var task8 = SearchByGeo(mongoClient);
        //     var task9 = SearchByGeo(mongoClient);
        //     var task10 = SearchByGeo(mongoClient);
        //     var task11 = SearchByGeo(mongoClient);
        //     var task12 = SearchByGeo(mongoClient);
        //     var task13 = SearchByGeo(mongoClient);
        //     var task14 = SearchByGeo(mongoClient);
        //     var task15 = SearchByGeo(mongoClient);
        //     var task16 = SearchByGeo(mongoClient);

        //     await Task.WhenAll(task1, task2, task3, task4, task5, task6, task7, task8, task9, task10, task11, task12, task13, task14, task15, task16);
        // }

        // await SearchAndPaginateGeoAndProduct(mongoClient);

        await AddDirectoryFieldsToBlobs(mongoClient);
        // await TestDirectorySearch(mongoClient);
    }

    static async Task FillDatabaseWithMetadata(BlobServiceClient blobServiceClient, MongoClient mongoClient)
    {
        Console.WriteLine("Azure blob containers:");
        List<string> containers = [];
        await foreach (var containerName in blobServiceClient.GetBlobContainersAsync())
        {
            Console.WriteLine(containerName);

            var containerClient = blobServiceClient.GetBlobContainerClient("2023");
            var blobDatabase = mongoClient.GetDatabase("blobs");
            var collection = blobDatabase.GetCollection<BsonDocument>("BlobsWithGeometry");

            int blobsScanned = 0;
            var loggingTask = LogThroughputAsync(() => blobsScanned);

            await foreach (var blobPage in containerClient.GetBlobsAsync(BlobTraits.Metadata).AsPages())
            {
                List<BsonDocument> documentsToInsert = new List<BsonDocument>();
                foreach (var blobItem in blobPage.Values)
                {
                    if (blobItem.Metadata == null || blobItem.Metadata.Count == 0 || blobItem.Metadata.ContainsKey("geometry") == false)
                    {
                        continue;
                    }

                    var doc = new BsonDocument
                    {
                        { "BlobName", blobItem.Name },
                        { "ContentType", blobItem.Properties.ContentType },
                        { "BlobSize", blobItem.Properties.ContentLength },
                    };

                    foreach (var key in blobItem.Metadata.Keys)
                    {
                        if (KEYS_TO_IGNORE.Contains(key))
                        {
                            continue;
                        }

                        if (key == "geometry")
                        {
                            doc.Add(key, GetGeometry(blobItem.Metadata[key]));
                            continue;
                        }
                        else
                        {
                            doc.Add(key, blobItem.Metadata[key]);
                        }
                    }

                    documentsToInsert.Add(doc);
                    Interlocked.Increment(ref blobsScanned);
                }

                if (documentsToInsert.Count > 0)
                {
                    await collection.InsertManyAsync(documentsToInsert);
                }
            }
        }
    }

    public static async Task CountDocuments(MongoClient mongoClient)
    {
        var blobDatabase = mongoClient.GetDatabase("blobs");
        var collection = blobDatabase.GetCollection<BsonDocument>("BlobMetadata");

        var watch = Stopwatch.StartNew();
        var count = await collection.CountDocumentsAsync(new BsonDocument());
        Console.WriteLine($"Total documents: {count}");
        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task CountDocumentsWithGeometry(MongoClient mongoClient)
    {
        var blobDatabase = mongoClient.GetDatabase("blobs");
        var collection = blobDatabase.GetCollection<BsonDocument>("BlobMetadata");

        var filter = Builders<BsonDocument>.Filter.Exists("geometry");
        var watch = Stopwatch.StartNew();
        var count = await collection.CountDocumentsAsync(filter);
        Console.WriteLine($"Documents with geometry: {count}");
        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task CountDocumentsWithoutGeometry(MongoClient mongoClient)
    {
        var blobDatabase = mongoClient.GetDatabase("blobs");
        var collection = blobDatabase.GetCollection<BsonDocument>("BlobMetadata");

        var filter = Builders<BsonDocument>.Filter.Exists("geometry", false);
        var watch = Stopwatch.StartNew();
        var count = await collection.CountDocumentsAsync(filter);
        Console.WriteLine($"Documents without geometry: {count}");
        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task AggregationByMetadataExample(MongoClient mongoClient)
    {
        var blobDatabase = mongoClient.GetDatabase("blobs");
        var collection = blobDatabase.GetCollection<BsonDocument>("BlobsWithGeometry");

        var watch = Stopwatch.StartNew();

        BsonDocument match = new BsonDocument
        {
            {
                "$match", new BsonDocument{
                    { "resolution", "7.5 CM" },
                    { "jaartal", "2023"},
                    { "color", "RGB" }
                }
            }
        };

        var group = new BsonDocument("$group",
            new BsonDocument
            {
                { "_id", BsonNull.Value },  // Group all documents together
                { "totalBlobSize", new BsonDocument("$sum", "$BlobSize") }
            });

        var pipeline = new[] { match, group };

        var aggregateFluent = collection.Aggregate<BsonDocument>(pipeline);

        var results = await aggregateFluent.ToListAsync();

        if (results.Any())
        {
            Console.WriteLine("Total Blob Size: " + FormatBytes(results.First()["totalBlobSize"].ToInt64()));
        }
        else
        {
            Console.WriteLine("No documents found.");
        }

        // var firstFoundDocument = await collection.Find(match).FirstOrDefaultAsync();
        // Console.WriteLine("First found document: " + firstFoundDocument);

        watch.Stop();

        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public async Task FilterByMetadataExample(MongoClient mongoClient)
    {
        var blobDatabase = mongoClient.GetDatabase("blobs");
        var collection = blobDatabase.GetCollection<BsonDocument>("BlobMetadata");

        var filterBuilder = Builders<BsonDocument>.Filter;
        var filter = filterBuilder.And(
            filterBuilder.Eq("resolution", "7.5 CM"),
            filterBuilder.Eq("jaartal", "2023"),
            filterBuilder.Eq("color", "RGB"),
            filterBuilder.Exists("geometry"));

        var findOptions = new FindOptions<BsonDocument>
        {
            BatchSize = 1000, // Adjust batch size based on your memory and performance needs
            NoCursorTimeout = false, // This option prevents the cursor from timing out during long operations.
            Projection = Builders<BsonDocument>.Projection.Exclude("ContentType"),
        };

        var watch = Stopwatch.StartNew();

        using var cursor = await collection.FindAsync(filter, findOptions);

        while (await cursor.MoveNextAsync())
        {
            var batch = cursor.Current;
            foreach (var document in batch)
            {
                Console.WriteLine(document.ToString());
            }
        }

        watch.Stop();

        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");

        var firstFoundDocument = await collection.Find(filter).FirstOrDefaultAsync();
        Console.WriteLine("First found document: " + firstFoundDocument);
    }

    public static async Task CopyGeometryToProperGeometry(MongoClient mongoClient, int offset = 0)
    {
        var database = mongoClient.GetDatabase("blobs");
        var srcCollection = database.GetCollection<BsonDocument>("BlobMetadata");
        var dstCollection = database.GetCollection<BsonDocument>("BlobMetadataNewGeometry");
        var filter = Builders<BsonDocument>.Filter.Exists("geometry");
        var findOptions = new FindOptions
        {
            BatchSize = 1000,
            NoCursorTimeout = false,
        };

        var watch = Stopwatch.StartNew();

        var documents = await srcCollection.Find(filter, findOptions).ToCursorAsync();
        var numDocsInserted = 0;

        var documentBatch = new List<BsonDocument>();
        foreach (var doc in documents.ToEnumerable())
        {
            var json = JsonDocument.Parse(doc["geometry"].ToString());

            JsonElement root = json.RootElement;
            JsonElement features = root.GetProperty("features");
            JsonElement firstFeature = features[0];
            JsonElement geometry = firstFeature.GetProperty("geometry");
            var type = geometry.GetProperty("type").GetString();
            JsonElement coordinates = geometry.GetProperty("coordinates");

            var newGeoDoc = new BsonDocument
            {
                { "type", type },
                { "coordinates", BsonArray.Create(coordinates.EnumerateArray().SelectMany(x => x.EnumerateArray().Select(y => new BsonArray { y[0].GetDecimal() + offset, y[1].GetDecimal() + offset }))) }
            };

            var newDoc = new BsonDocument
            {
                { "BlobName", doc["BlobName"] },
                { "ContentType", doc["ContentType"] },
                { "BlobSize", doc["BlobSize"] },
                { "geometryNew", newGeoDoc },
            };

            documentBatch.Add(newDoc);

            if (documentBatch.Count > 1000)
            {
                numDocsInserted += documentBatch.Count;
                await dstCollection.InsertManyAsync(documentBatch);
                documentBatch.Clear();
                Console.WriteLine($"Inserted {numDocsInserted} documents");
            }
        }

        if (documentBatch.Count > 0)
        {
            numDocsInserted += documentBatch.Count;
            await dstCollection.InsertManyAsync(documentBatch);
            Console.WriteLine($"Inserted {numDocsInserted} documents");
        }

        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task CopyGeometryToEPSG4326(MongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("blobs");
        var collection = database.GetCollection<BsonDocument>("BlobsWithGeometry");

        var watch = Stopwatch.StartNew();
        var filter = Builders<BsonDocument>.Filter.Exists("geometry4326.type", false);
        var documents = await collection.Find(filter).ToCursorAsync();

        var transformer = new Transformer();
        int blobsScanned = 0;
        var loggingTask = LogThroughputAsync(() => blobsScanned);

        var bulkOps = new List<WriteModel<BsonDocument>>();

        foreach (var doc in documents.ToEnumerable())
        {
            if (doc.Contains("geometry") && doc["geometry"].AsBsonDocument.Contains("coordinates"))
            {
                var coordinatesField = doc["geometry"]["coordinates"].AsBsonArray;
                List<double[]> polygonPoints = [];

                foreach (BsonArray coord in coordinatesField.Cast<BsonArray>())
                {
                    double x = coord[0].ToDouble();
                    double y = coord[1].ToDouble();
                    polygonPoints.Add(new double[] { x, y });
                }

                // Transform the points
                var transformedPoints = transformer.TransformPolygon(polygonPoints);

                var updateDefinition = Builders<BsonDocument>.Update
                    .Set("geometry4326.coordinates", transformedPoints)
                    .Set("geometry4326.type", "Polygon");

                var updateOneModel = new UpdateOneModel<BsonDocument>(
                    Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]), // Filter
                    updateDefinition // Update
                );

                // Add to bulk operations
                bulkOps.Add(updateOneModel);

                // await collection.UpdateOneAsync(
                //     Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]),
                //     updateDefinition
                // );

                Interlocked.Increment(ref blobsScanned);
            }

            if (bulkOps.Count > 0)
            {
                await collection.BulkWriteAsync(bulkOps);
                bulkOps.Clear();
            }
        }

        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task SearchByGeo(MongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("blobs");
        var collection = database.GetCollection<BsonDocument>("lvb-deliverables-2023");

        var findOptions = new FindOptions
        {
            BatchSize = 1000,
            NoCursorTimeout = true,
        };
        // var documents = await collection.Find(filter, findOptions).ToListAsync();

        var nl = Json2Polygon("kaartselecties/nederland-EPSG4326.json");
        var groot = Json2Polygon("kaartselecties/groot.json");
        var limburg = Json2Polygon("kaartselecties/limburg-EPSG4326.json");
        var friesland = Json2Polygon("kaartselecties/friesland-EPSG4326.json");
        var zuidholl = Json2Polygon("kaartselecties/zuidholland-EPSG4326.json");

        // these all have overlap in one area
        var blokindeling = Json2Polygon("kaartselecties/blokindeling-EPSG4326.json");
        var rivierenland1 = Json2Polygon("kaartselecties/rivierenland1-EPSG4326.json");
        var rivierenland2 = Json2Polygon("kaartselecties/rivierenland2-EPSG4326.json");
        var rijnijssel = Json2Polygon("kaartselecties/rijnijssel-EPSG4326.json");

        var geoFilter1 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", groot),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", nl),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", blokindeling),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland1),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland2),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rijnijssel)
        );

        var watch = Stopwatch.StartNew();
        // var count = await collection.CountDocumentsAsync(geoFilter1);
        // watch.Stop();
        // Console.WriteLine($"Execution time filter1 is {watch.ElapsedMilliseconds} ms. Found {count} documents");

        var geoFilter2 = Builders<BsonDocument>.Filter.Or(
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", nl),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", groot),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", limburg),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", friesland),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", zuidholl),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", blokindeling),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland1),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland2),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rijnijssel)
        );

        // watch.Restart();
        // count = await collection.CountDocumentsAsync(geoFilter2);
        // watch.Stop();
        // Console.WriteLine($"Execution time filter2 is {watch.ElapsedMilliseconds} ms. Found {count} documents");

        var productFilter1 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "7.5 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "RGB")
        );

        var productFilter2 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "25 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "CIR")
        );

        var productFilter3 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "25 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "RGB")
        );

        var productFilter4 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "7.5 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "CIR")
        );

        // Also search on a few other properties, this simulates searching for products + geometry.
        var geoAndProductFilter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Or(geoFilter1, geoFilter2),
            Builders<BsonDocument>.Filter.Or(productFilter1, productFilter2, productFilter3, productFilter4)
        );

        watch.Restart();
        var count = await collection.CountDocumentsAsync(geoAndProductFilter);
        watch.Stop();
        Console.WriteLine($"Execution time filter3 is {watch.ElapsedMilliseconds} ms. Found {count} documents");
    }

    public static async Task SearchAndPaginateGeoAndProduct(MongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("blobs");
        var collection = database.GetCollection<BsonDocument>("lvb-deliverables-2023");

        var findOptions = new FindOptions
        {
            BatchSize = 100,
            NoCursorTimeout = true,
        };

        var nl = Json2Polygon("kaartselecties/nederland-EPSG4326.json");
        var blokindeling = Json2Polygon("kaartselecties/blokindeling-EPSG4326.json");
        var rivierenland1 = Json2Polygon("kaartselecties/rivierenland1-EPSG4326.json");
        var rivierenland2 = Json2Polygon("kaartselecties/rivierenland2-EPSG4326.json");
        var rijnijssel = Json2Polygon("kaartselecties/rijnijssel-EPSG4326.json");

        var geoFilter1 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", nl),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", blokindeling),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland1),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rivierenland2),
            Builders<BsonDocument>.Filter.GeoIntersects("geometry.coordinates", rijnijssel)
        );

        var productFilter1 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "7.5 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "RGB"),
            Builders<BsonDocument>.Filter.Eq("extension", ".ecw")
        );

        var productFilter2 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "25 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "CIR"),
            Builders<BsonDocument>.Filter.Eq("extension", ".ecw")
        );

        var productFilter3 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "25 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "RGB"),
            Builders<BsonDocument>.Filter.Eq("extension", ".ecw")
        );

        var productFilter4 = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("resolution", "7.5 CM"),
            Builders<BsonDocument>.Filter.Eq("jaartal", "2023"),
            Builders<BsonDocument>.Filter.Eq("color", "CIR"),
            Builders<BsonDocument>.Filter.Eq("extension", ".ecw")
        );

        // Also search on a few other properties, this simulates searching for products + geometry.
        var geoAndProductFilter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Or(geoFilter1),
            Builders<BsonDocument>.Filter.Or(productFilter1, productFilter2, productFilter3, productFilter4)
        );

        int currentPage = 0;
        int pageSize = 30;

        while (true)
        {
            var documents = await collection
                .Find(geoAndProductFilter, findOptions)
                .SortBy(x => x["BlobName"])
                .Skip(currentPage * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            if (documents.Count == 0)
            {
                break;
            }

            Console.WriteLine($"Page {currentPage + 1}");
            foreach (var doc in documents)
            {
                Console.WriteLine($"  {doc["BlobName"]}");
            }

            currentPage++;
        }
    }

    public static async Task AddDirectoryFieldsToBlobs(MongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("blobs");
        var collection = database.GetCollection<BsonDocument>("lvb-deliverables-2023");

        var watch = Stopwatch.StartNew();
        var documents = await collection.AsQueryable().ToCursorAsync();

        int blobsScanned = 0;
        var loggingTask = LogThroughputAsync(() => blobsScanned);

        var bulkOps = new List<WriteModel<BsonDocument>>();

        foreach (var doc in documents.ToEnumerable())
        {
            if (doc.Contains("BlobName"))
            {
                var blobName = doc["BlobName"].AsString;
                string dir = Path.Join(blobName.Split('/')[0..^1]);
                string parentDir = Path.Join(blobName.Split('/')[0..^2]);

                var updateDefinition = Builders<BsonDocument>.Update
                    .Set("_dir", dir)
                    .Set("_parentDir", parentDir)
                    .Set("_segments", blobName.Split('/')[0..^1]);

                var updateOneModel = new UpdateOneModel<BsonDocument>(
                    Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]),
                    updateDefinition
                );

                // Add to bulk operations
                bulkOps.Add(updateOneModel);

                // await collection.UpdateOneAsync(
                //     Builders<BsonDocument>.Filter.Eq("_id", doc["_id"]),
                //     updateDefinition
                // );

                Interlocked.Increment(ref blobsScanned);
            }

            if (bulkOps.Count > 0)
            {
                await collection.BulkWriteAsync(bulkOps);
                bulkOps.Clear();
            }
        }

        watch.Stop();
        Console.WriteLine($"The Execution time of the program is {watch.ElapsedMilliseconds} ms");
    }

    public static async Task TestDirectorySearch(MongoClient mongoClient)
    {
        // kunnen we binnen reele tijd zoeken op een gedeelte in het blob name/path veld?
        // elke blob heeft _dir en _parentDir veld.
        // vb: { BlobName: "Ortho/5/ortho_5_2023_25cm_RGB.ecw", _dir: "Ortho/5", _parentDir: "Ortho" }

        // gebruik _parentDir om te vragen
        // 1. geef alle directories binnen het pad x, moet met een unique aggregatie query kunnen

        // gebruik _dir om te vragen
        // 2. geef alle blobs binnen het pad x

        var database = mongoClient.GetDatabase("blobs");
        var collection = database.GetCollection<BsonDocument>("lvb-deliverables-2023");

        var findOptions = new FindOptions
        {
            BatchSize = 100,
            NoCursorTimeout = true,
        };

        int currentPage = 0;
        int pageSize = 30;

        // 1. geef alle directories binnen het pad x
        // 1a. dit zijn alle blobs daarbinnen.
        var filter = Builders<BsonDocument>.Filter.Eq("_parentDir", "Ortho/1");

        // 1b. maak een aggregatie query om alle unieke directories te vinden
        var directories = await collection
            .DistinctAsync<string>("_dir", filter);

        foreach (var dir in await directories.ToListAsync())
        {
            Console.WriteLine(dir);
        }

        // Dit is niet genoeg, omdat je nu vanuit de root niet overal kan komen als
        // er niet op elk niveau minstens een blob is.

        var distinctFirstCoords = await collection
            .DistinctAsync<double>("geometry.coordinates.0", Builders<BsonDocument>.Filter.Exists("geometry.coordinates.0"));

        foreach (var coord in await distinctFirstCoords.ToListAsync())
        {
            Console.WriteLine(coord);
        }
    }

    private static GeoJsonGeometry<GeoJson2DCoordinates> Json2Polygon(string jsonPath)
    {
        var json = JsonDocument.Parse(File.ReadAllText(jsonPath));

        JsonElement root = json.RootElement;
        JsonElement features = root.GetProperty("features");
        JsonElement firstFeature = features[0];
        JsonElement geometry = firstFeature.GetProperty("geometry");
        var type = geometry.GetProperty("type").GetString(); // Polygon
        JsonElement coordinatesJson = geometry.GetProperty("coordinates");

        var coordinates = coordinatesJson.EnumerateArray().SelectMany(x => x.EnumerateArray().Select(y => new GeoJson2DCoordinates(y[0].GetDouble(), y[1].GetDouble()))).ToArray();
        return GeoJson.Polygon(coordinates);
    }

    private static async Task LogThroughputAsync(Func<int> counterMethod)
    {
        var lastCount = 0;
        while (true)
        {
            await Task.Delay(10000);
            var currentCount = counterMethod();
            var throughput = currentCount - lastCount;
            lastCount = currentCount;
            Console.WriteLine($"Throughput: {throughput * 6} operations/minute");
        }
    }

    public static string FormatBytes(long bytes)
    {
        string[] sizes = { "Bytes", "KB", "MB", "GB", "TB" };
        double len = bytes;
        int order = 0;

        while (len >= 1024 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1024;
        }

        return String.Format("{0:0.##} {1}", len, sizes[order]);
    }

    private static BsonDocument GetGeometry(string jsonStr)
    {
        var json = JsonDocument.Parse(jsonStr);

        JsonElement root = json.RootElement;
        JsonElement features = root.GetProperty("features");
        JsonElement firstFeature = features[0];
        JsonElement geometry = firstFeature.GetProperty("geometry");
        var type = geometry.GetProperty("type").GetString();
        JsonElement coordinates = geometry.GetProperty("coordinates");

        return new BsonDocument
        {
            { "type", type },
            { "coordinates", BsonArray.Create(coordinates.EnumerateArray().SelectMany(x => x.EnumerateArray().Select(y => new BsonArray { y[0].GetDecimal(), y[1].GetDecimal() }))) }
        };
    }
}
