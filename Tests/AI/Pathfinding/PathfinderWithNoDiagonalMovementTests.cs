﻿using System.Collections.Immutable;
using FluentAssertions;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;
using Path = Turnable.AI.Pathfinding.Path;

namespace Tests.AI.Pathfinding;

public class PathfinderWithNoDiagonalMovementTests
{
    private PathfinderFunc? _pathfinder;

    [Fact]
    internal void Getting_a_pathfinder_from_a_map()
    {
        MapFilePath mapFilePath =
            new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map sut = mapJsonString.Deserialize();

        PathfinderFunc pathfinder = sut.GetPathfinderFunc(0, new[]{1}, allowDiagonal: false);

        pathfinder.Should().NotBeNull();
    }

    [Fact]
    internal void Finding_a_path_between_two_locations_next_to_each_other()
    {
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(1, 1), new Location(2, 1));

        path.Should().NotBeNull();
        path.Count.Should().Be(2);
        path[0].Should().Be(new Location(1, 1));
        path[1].Should().Be(new Location(2, 1));
    }

    [Fact]
    internal void Finding_a_path_between_two_locations_horizontal_to_each_other()
    {
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(1, 1), new Location(4, 1));

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
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(4, 1), new Location(4, 4));

        path.Should().NotBeNull();
        path.Count.Should().Be(4);
        path[0].Should().Be(new Location(4, 1));
        path[1].Should().Be(new Location(4, 2));
        path[2].Should().Be(new Location(4, 3));
        path[3].Should().Be(new Location(4, 4));
    }

    [Fact]
    internal void Finding_a_moderately_complex_path_between_two_locations_horizontal_to_each_other_and_obstacles_in_between()
    {
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(10, 13), new Location(14, 13));

        path.Should().NotBeNull();
        path.Count.Should().Be(9);
        path[0].Should().Be(new Location(10, 13));
        path[1].Should().Be(new Location(10, 12));
        path[2].Should().Be(new Location(11, 12));
        path[3].Should().Be(new Location(11, 11));
        path[4].Should().Be(new Location(12, 11));
        path[5].Should().Be(new Location(13, 11));
        path[6].Should().Be(new Location(13, 12));
        path[7].Should().Be(new Location(14, 12));
        path[8].Should().Be(new Location(14, 13));
    }

    [Fact]
    internal void Finding_a_path_when_none_exists()
    {
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(2, 3), new Location(4, 3));

        path.Should().NotBeNull();
        path.Count.Should().Be(0);
    }

    [Fact]
    internal void Finding_a_path_when_the_start_and_end_are_both_unwalkable()
    {
        PathfinderFunc sut = CreatePathfinderWithNoDiagonalPathsAllowed();

        Path path = sut(new Location(1, 3), new Location(3, 3));

        path.Should().NotBeNull();
        path.Count.Should().Be(0);
    }

    // Factory method to create the pathfinder, and cache it for future calls
    private PathfinderFunc CreatePathfinderWithNoDiagonalPathsAllowed()
    {
        if (_pathfinder != null) return _pathfinder;

        MapFilePath mapFilePath =
            new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        _pathfinder = map.GetPathfinderFunc(0, new[]{1}, allowDiagonal: false);

        return _pathfinder;
    }
}
