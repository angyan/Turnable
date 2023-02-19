﻿using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Turnable.Places;

internal static class Level
{
    internal static Bounds GetBounds(this Map map) => new(new Location(0, 0), new Dimension(map.Width), new Dimension(map.Height));

    internal static Bounds GetBounds(this Layer layer) => new(new Location(0, 0), new Dimension(layer.Width), new Dimension(layer.Height));

    internal static ImmutableList<Location> GetObstacles(this Layer layer) =>
        (from location in layer.GetBounds().GetLocations()
            where layer.TileGidAt(location) != 0
            select location)
        .ToImmutableList();

    internal static ImmutableDictionary<Location, ImmutableList<Location>> NodesAnWalkableNeighbors(this Layer layer, ImmutableList<Layer> obstacleLayers)
    {
        IImmutableSet<Location> allObstacles = obstacleLayers.SelectMany(l => l.GetObstacles()).ToImmutableHashSet();
        bool IncludeFunc(Bounds bounds, Location location) => !allObstacles.Contains(location);

        Dictionary<Location, ImmutableList<Location>> dictionary =
            (from location in layer.GetBounds().GetLocations()
                let walkableNeighbors = layer.GetBounds().GetContainedNeighbors(location, IncludeFunc)
                select new KeyValuePair<Location, ImmutableList<Location>>(location,
                    walkableNeighbors.ToImmutableList())).ToDictionary(pair => pair.Key, pair => pair.Value);

        return dictionary.ToImmutableDictionary();
    }
}