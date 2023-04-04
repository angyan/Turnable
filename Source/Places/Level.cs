using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.TiledMap;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Turnable.Places;

public static class Level
{
    internal static Bounds GetBounds(this Map map) => new(new(0, 0), new(map.Width, map.Height));

    internal static Bounds GetBounds(this Layer layer) => new(new(0, 0), new(layer.Width, layer.Height));

    public static int GetTileGid(this Layer layer, Location location)
    {
        return layer.Data[location.X + location.Y * layer.Width];
    }

    public static int GetDataIndex(Location location, int width)
    {
        return location.X + location.Y * width;
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

    internal static ImmutableList<Location> GetObstacles(this Map map, CollisionMasks collisionMasks) => (
        collisionMasks.Value.SelectMany(maskLayerIndex => map.Layers[maskLayerIndex].GetObstacles())).ToImmutableList();

    private static Graph ConstructGraph(this Map map, int layerIndex, Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc,
        Func<Location, bool> includeFunc)
    {
        Layer layer = map.Layers[layerIndex];

        Dictionary<Location, ImmutableList<Location>> dictionary =
            (from location in layer.GetBounds().GetLocations()
                let bounds = layer.GetBounds()
                let walkableNeighbors = getNeighborsFunc(bounds, location, includeFunc)
                select new KeyValuePair<Location, ImmutableList<Location>>(location,
                    walkableNeighbors.ToImmutableList())).ToDictionary(pair => pair.Key, pair => pair.Value);

        return new Graph(dictionary.ToImmutableDictionary());
    }

    private static Func<Location, bool> IncludeOnlyFreeLocationsFunc(IList<Location> obstacles)
    {
        return (location) => !obstacles.Contains(location);
    }

    private static Graph ConstructGraph(this Map map, int layerIndex, CollisionMasks collisionMasks,
        Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc) => ConstructGraph(map,
        layerIndex, getNeighborsFunc, IncludeOnlyFreeLocationsFunc(map.GetObstacles(collisionMasks)));

    private static Graph ConstructGraph(this Map map, int layerIndex, CollisionMasks collisionMasks,
        Func<Bounds, Location, Func<Location, bool>, ImmutableList<Location>> getNeighborsFunc,
        IList<Location> additionalObstacles) => ConstructGraph(map,
        layerIndex, getNeighborsFunc,
        IncludeOnlyFreeLocationsFunc(map.GetObstacles(collisionMasks).AddRange(additionalObstacles)));

    public static Graph GetGraph(this Map map, int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal) =>
        (allowDiagonal
            ? map.ConstructGraph(layerIndex, collisionMasks, Grid.GetContainedNeighbors)
            : map.ConstructGraph(layerIndex, collisionMasks, Grid.GetContainedNonDiagonalNeighbors));

    public static Graph GetGraph(this Map map, int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal, IList<Location> additionalObstacles) =>
        (allowDiagonal
            ? map.ConstructGraph(layerIndex, collisionMasks, Grid.GetContainedNeighbors, additionalObstacles)
            : map.ConstructGraph(layerIndex, collisionMasks, Grid.GetContainedNonDiagonalNeighbors, additionalObstacles));
}