using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;

namespace Turnable.TiledMap;

public record Map(int CompressionLevel, int Height, bool Infinite, Layer[] Layers, int NextLayerId,
    int NextObjectId,
    string Orientation, string RenderOrder, string TiledVersion, int TileHeight, Tileset[] Tilesets, int TileWidth,
    string Type, string Version, int Width)
{
    internal Map Load(string filePath)
    {
        MapFilePath mapFilePath = new MapFilePath(filePath);
        MapJsonString mapJsonString = new MapJsonString(File.ReadAllText(mapFilePath));

        return mapJsonString.Deserialize();
    }

    internal Bounds Bounds => new(new(0, 0), new(Width, Height));

    internal ImmutableList<Location> GetObstacles(CollisionMasks collisionMasks) => (
        collisionMasks.Value.SelectMany(maskLayerIndex => Layers[maskLayerIndex].GetObstacles())).ToImmutableList();

    private Graph ConstructGraph(int layerIndex, bool allowDiagonal, Func<Location, bool> includeFunc)
    {
        Layer layer = Layers[layerIndex];

        Dictionary<Location, ImmutableList<Location>> dictionary =
            (from location in layer.Bounds.GetLocations()
                let bounds = layer.Bounds
                let walkableNeighbors = Layouts.Bounds.GetNeighbors(bounds, location, allowDiagonal, includeFunc)
                select new KeyValuePair<Location, ImmutableList<Location>>(location,
                    walkableNeighbors.ToImmutableList())).ToDictionary(pair => pair.Key, pair => pair.Value);

        return new Graph(dictionary.ToImmutableDictionary());
    }

    private Func<Location, bool> IncludeOnlyFreeLocationsPredicateFunc(IList<Location> obstacles) =>
        (location) => !obstacles.Contains(location);

    private Graph ConstructGraph(int layerIndex, CollisionMasks collisionMasks
        , bool allowDiagonal) => ConstructGraph(
        layerIndex, allowDiagonal, IncludeOnlyFreeLocationsPredicateFunc(GetObstacles(collisionMasks)));

    private Graph ConstructGraph(int layerIndex, CollisionMasks collisionMasks, bool allowDiagonal,
        IList<Location> additionalObstacles) => ConstructGraph(
        layerIndex, allowDiagonal,
        IncludeOnlyFreeLocationsPredicateFunc(GetObstacles(collisionMasks).AddRange(additionalObstacles)));

    public Graph GetGraph(int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal) => ConstructGraph(layerIndex, collisionMasks, allowDiagonal);

    public Graph GetGraph(int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal, IList<Location> additionalObstacles) => ConstructGraph(layerIndex, collisionMasks, allowDiagonal, additionalObstacles);
};
