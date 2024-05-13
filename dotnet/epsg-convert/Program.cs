#pragma warning disable CS8602

using System.Text.Json;
using System.Text.Json.Nodes;

namespace EpsgConvert;


internal class Program
{
    private static void Main(string[] args)
    {
        Example();
        ConvertJsonFile("rectangle-EPSG28992.json");
        ConvertJsonFile("nederland-EPSG28992.json");
    }

    private static void Example()
    {
        Console.WriteLine("Test out projection conversions.");

        var transformer = new Transformer();

        // Example polygon points in EPSG:28992
        var polygonPoints = new List<double[]>
        {
            new double[] { 196105, 317073 },
            new double[] { 196055, 317018 },
            new double[] { 196015, 317058 }
        };

        var transformedPolygon = transformer.TransformPolygon(polygonPoints);

        foreach (var point in transformedPolygon)
        {
            Console.WriteLine($"Transformed Point: Latitude {point[1]}, Longitude {point[0]}");
        }
    }

    private static void ConvertJsonFile(string filename)
    {
        var transformer = new Transformer();

        var jsonStr = File.ReadAllText(filename);
        var json = JsonSerializer.Deserialize<JsonNode>(jsonStr);
        var features = json["features"].AsArray();
        var coordinates = features[0]["geometry"]["coordinates"][0] as JsonArray;

        var newCoordinates = new JsonArray();
        foreach (var point in coordinates)
        {
            var pointArray = point.AsArray();
            var lat = pointArray[1].GetValue<double>();
            var lon = pointArray[0].GetValue<double>();
            var transformed = transformer.TransformPolygon([[lon, lat]])[0];
            newCoordinates.Add(new JsonArray(transformed[0], transformed[1]));
        }

        features[0]["geometry"]["coordinates"][0] = newCoordinates;

        // Writing updated JSON to a new file
        var updatedJsonStr = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });

        File.WriteAllText(filename.Replace("28992", "4326"), updatedJsonStr);
    }
}
