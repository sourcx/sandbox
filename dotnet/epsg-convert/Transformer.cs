using DotSpatial.Projections;

namespace EpsgConvert;

public class Transformer
{
    private readonly ProjectionInfo _sourceProjection;
    private readonly ProjectionInfo _targetProjection;

    public Transformer()
    {
        _sourceProjection = KnownCoordinateSystems.Projected.NationalGrids.DutchRD;
        _targetProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
    }

    public List<double[]> TransformPolygon(List<double[]> polygonPoints)
    {
        var transformedPoints = new List<double[]>();

        foreach (var point in polygonPoints)
        {
            double[] transformedPoint = [point[0], point[1]];
            Reproject.ReprojectPoints(transformedPoint, null, _sourceProjection, _targetProjection, 0, 1);
            transformedPoints.Add(transformedPoint);
        }

        return transformedPoints;
    }
}
