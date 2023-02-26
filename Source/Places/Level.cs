using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Turnable.Places;

public static class Level
{
    internal static Bounds GetBounds(this Map map) => new(new Location(0, 0), new Dimension(map.Width), new Dimension(map.Height));

    internal static Bounds GetBounds(this Layer layer) => new(new Location(0, 0), new Dimension(layer.Width), new Dimension(layer.Height));

    public static int GetTileGid(this Layer layer, Location location)
    {
        return layer.Data[location.X + location.Y * layer.Width];
    }

    public static Location GetAtlasLocation(this Tileset tileset, int tileGlobalId)
    {
        int tileId = tileGlobalId - tileset.FirstGid;
        int atlasX = tileId % tileset.Columns;
        int atlasY = tileId / tileset.Columns;

        return new(atlasX, atlasY);
    }

    internal static ImmutableList<Location> GetObstacles(this Layer layer) =>
        (from location in layer.GetBounds().GetLocations()
            where layer.GetTileGid(location) != 0
            select location)
        .ToImmutableList();

    private static ImmutableDictionary<Location, ImmutableList<Location>> ConstructGraph(this Layer layer, ImmutableList<Layer> obstacleLayers, Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc)
    {
        IImmutableSet<Location> allObstacles = obstacleLayers.SelectMany(l => l.GetObstacles()).ToImmutableHashSet();
        bool IncludeFunc(Location location) => !allObstacles.Contains(location);

        Dictionary<Location, ImmutableList<Location>> dictionary =
            (from location in layer.GetBounds().GetLocations()
                let bounds = layer.GetBounds()
                let walkableNeighbors = getNeighborsFunc(bounds, location, IncludeFunc)
                select new KeyValuePair<Location, ImmutableList<Location>>(location,
                    walkableNeighbors.ToImmutableList())).ToDictionary(pair => pair.Key, pair => pair.Value);

        return dictionary.ToImmutableDictionary();
    }

    public static ImmutableDictionary<Location, ImmutableList<Location>> GetGraph(this Layer layer,
        ImmutableList<Layer> obstacleLayers, bool allowDiagonal) =>
        (allowDiagonal
            ? ConstructGraph(layer, obstacleLayers, Grid.GetContainedNeighbors)
            : ConstructGraph(layer, obstacleLayers, Grid.GetContainedNonDiagonalNeighbors));
}