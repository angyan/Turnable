using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
namespace Turnable.TiledMap;

public record Map(int CompressionLevel, int Height, bool Infinite, Layer[] Layers, int NextLayerId,
    int NextObjectId,
    string Orientation, string RenderOrder, string TiledVersion, int TileHeight, Tileset[] Tilesets, int TileWidth,
    string Type, string Version, int Width)
{
    internal Bounds Bounds() => new(new(0, 0), new(this.Width, this.Height));

    internal ImmutableList<Location> Obstacles(CollisionMasks collisionMasks) => (
        collisionMasks.Value.SelectMany(maskLayerIndex => this.Layers[maskLayerIndex].Obstacles())).ToImmutableList();

    private Graph ConstructGraph(int layerIndex,
        Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc,
        Func<Location, bool> includeFunc)
    {
        Layer layer = this.Layers[layerIndex];

        Dictionary<Location, ImmutableList<Location>> dictionary =
            (from location in layer.Bounds().GetLocations()
                let bounds = layer.Bounds()
                let walkableNeighbors = getNeighborsFunc(bounds, location, includeFunc)
                select new KeyValuePair<Location, ImmutableList<Location>>(location,
                    walkableNeighbors.ToImmutableList())).ToDictionary(pair => pair.Key, pair => pair.Value);

        return new Graph(dictionary.ToImmutableDictionary());
    }

    private Func<Location, bool> IncludeOnlyFreeLocationsFunc(IList<Location> obstacles) =>
        (location) => !obstacles.Contains(location);

    private Graph ConstructGraph(int layerIndex, CollisionMasks collisionMasks,
        Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc) => ConstructGraph(
        layerIndex, getNeighborsFunc, IncludeOnlyFreeLocationsFunc(this.Obstacles(collisionMasks)));

    private Graph ConstructGraph(int layerIndex, CollisionMasks collisionMasks,
        Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc,
        IList<Location> additionalObstacles) => ConstructGraph(
        layerIndex, getNeighborsFunc,
        IncludeOnlyFreeLocationsFunc(this.Obstacles(collisionMasks).AddRange(additionalObstacles)));

    public Graph Graph(int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal) =>
        (allowDiagonal
            ? this.ConstructGraph(layerIndex, collisionMasks, Layouts.Bounds.GetContainedNeighbors)
            : this.ConstructGraph(layerIndex, collisionMasks, Layouts.Bounds.GetContainedNonDiagonalNeighbors));

    public Graph Graph(int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal, IList<Location> additionalObstacles) =>
        (allowDiagonal
            ? this.ConstructGraph(layerIndex, collisionMasks, Layouts.Bounds.GetContainedNeighbors, additionalObstacles)
            : this.ConstructGraph(layerIndex, collisionMasks, Layouts.Bounds.GetContainedNonDiagonalNeighbors,
                additionalObstacles));
};
