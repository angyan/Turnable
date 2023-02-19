using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.AI.Pathfinding;

public class PathfinderTests
{
    private Func<Location, Location, ImmutableList<Location>>? _pathfinder = null;

    [Fact]
    internal void Getting_a_pathfinder_from_a_pathfinder_context()
    {
        MapFilePath mapFilePath =
            new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        Layer layer = map.Layers[1];
        ImmutableDictionary<Location, ImmutableList<Location>> graph =
            layer.GetGraph(ImmutableList.Create<Layer>(map.Layers[1]));
        PathfinderContext sut = new PathfinderContext(graph);

        Func<Location, Location, ImmutableList<Location>> pathfinder = sut.GetPathfinder();

        pathfinder.Should().NotBeNull();
    }

    [Fact]
    internal void Finding_a_path_between_two_locations_next_to_each_other()
    {
        Func<Location, Location, ImmutableList<Location>> sut = CreatePathfinder();

        ImmutableList<Location> path = sut(new Location(1, 1), new Location(2, 1));

        path.Should().NotBeNull();
        path.Count.Should().Be(2);
        path[0].Should().Be(new Location(1, 1));
        path[1].Should().Be(new Location(2, 1));
    }

    [Fact]
    internal void Finding_a_path_between_two_locations_horizontal_to_each_other()
    {
        Func<Location, Location, ImmutableList<Location>> sut = CreatePathfinder();

        ImmutableList<Location> path = sut(new Location(1, 1), new Location(4, 1));

        path.Should().NotBeNull();
        path.Count.Should().Be(4);
        path[0].Should().Be(new Location(1, 1));
        path[1].Should().Be(new Location(2, 1));
        path[2].Should().Be(new Location(3, 1));
        path[3].Should().Be(new Location(4, 1));
    }

    [Fact]
    internal void Finding_a_path_between_two_locations_vertical_to_each_other()
    {
        Func<Location, Location, ImmutableList<Location>> sut = CreatePathfinder();

        ImmutableList<Location> path = sut(new Location(4, 1), new Location(4, 4));

        path.Should().NotBeNull();
        path.Count.Should().Be(4);
        path[0].Should().Be(new Location(4, 1));
        path[1].Should().Be(new Location(4, 2));
        path[2].Should().Be(new Location(4, 3));
        path[3].Should().Be(new Location(4, 4));
    }

    // Factory method to create the pathfinder, and cache it for future calls
    private Func<Location, Location, ImmutableList<Location>> CreatePathfinder()
    {
        if (_pathfinder != null) return _pathfinder;

        MapFilePath mapFilePath =
            new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        Layer layer = map.Layers[1];
        ImmutableDictionary<Location, ImmutableList<Location>> graph =
            layer.GetGraph(ImmutableList.Create<Layer>(map.Layers[1]));
        PathfinderContext pathfinderContext = new PathfinderContext(graph);
        _pathfinder = pathfinderContext.GetPathfinder();

        return _pathfinder;
    }
}
