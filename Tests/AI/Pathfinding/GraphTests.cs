using FluentAssertions;
using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;
using Path = Turnable.AI.Pathfinding.Path;

namespace Tests.AI.Pathfinding;

public class GraphTests
{
    [Fact]
    internal void An_indexer_exists_that_returns_neighbors_indexed_by_locations()
    {
        Dictionary<Location, ImmutableList<Location>> dictionary = new()
        {
            { new(1, 1), ImmutableList.Create(new Location(1, 2)) },
            { new(1, 2), ImmutableList.Create(new Location(1, 1)) }
        };

        Graph sut = new(dictionary.ToImmutableDictionary());

        sut[new(1, 1)].Should().BeEquivalentTo(ImmutableList.Create(new Location(1, 2)));
    }

    [Fact]
    internal void Returning_the_count_of_nodes()
    {
        Dictionary<Location, ImmutableList<Location>> dictionary = new()
        {
            { new(1, 1), ImmutableList.Create(new Location(1, 2)) },
            { new(1, 2), ImmutableList.Create(new Location(1, 1)) }
        };

        Graph sut = new(dictionary.ToImmutableDictionary());

        sut.Count.Should().Be(2);
    }

    public class PathfindingWithDiagonalNodesAllowedTests
    {
        [Fact]
        internal void Finding_a_path_between_two_locations_next_to_each_other()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 1), new Location(2, 1));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(2);
            path[0].Should().Be(new Location(1, 1));
            path[1].Should().Be(new Location(2, 1));
        }

        [Fact]
        internal void Finding_a_path_between_two_locations_horizontal_to_each_other()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 1), new Location(4, 1));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(4);
            path[0].Should().Be(new Location(1, 1));
            path[1].Should().Be(new Location(2, 1));
            path[2].Should().Be(new Location(3, 1));
            path[3].Should().Be(new Location(4, 1));
        }

        [Fact]
        internal void Finding_a_path_between_two_locations_vertical_to_each_other()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(4, 1), new Location(4, 4));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(4);
            path[0].Should().Be(new Location(4, 1));
            path[1].Should().Be(new Location(4, 2));
            path[2].Should().Be(new Location(4, 3));
            path[3].Should().Be(new Location(4, 4));
        }

        [Fact]
        internal void
            Finding_a_moderately_complex_path_between_two_locations_horizontal_to_each_other_and_obstacles_in_between()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(10, 13), new Location(14, 13));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(5);
            path[0].Should().Be(new Location(10, 13));
            path[1].Should().Be(new Location(11, 12));
            path[2].Should().Be(new Location(12, 11));
            path[3].Should().Be(new Location(13, 12));
            path[4].Should().Be(new Location(14, 13));
        }

        [Fact]
        internal void Finding_a_path_when_none_exists()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(2, 3), new Location(4, 3));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(0);
        }

        [Fact]
        internal void Finding_a_path_when_the_start_and_end_are_both_unwalkable()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 3), new Location(3, 3));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(0);
        }

        [Fact]
        public void Finding_a_path_from_one_location_to_three_locations_returns_three_paths_sorted_by_increasing_number_of_nodes()
        {
            Graph sut = CreateGraphWithDiagonalNodesAllowed();
            ImmutableList<Location> locations = ImmutableList.Create(new Location(1, 1), new Location(4, 4), new Location(2, 2));

            ImmutableList<Path> paths = sut.FindPaths(new Location(1, 1), locations);

            paths.Should().NotBeNull();
            paths.Count.Should().Be(3);
            paths[0].NodeCount.Should().BeLessOrEqualTo(paths[1].NodeCount);
            paths[1].NodeCount.Should().BeLessOrEqualTo(paths[2].NodeCount);
        }
    }

    public class PathfindingWithNoDiagonalNodesAllowedTests
    {
        [Fact]
        internal void Finding_a_path_between_two_locations_next_to_each_other()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 1), new Location(2, 1));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(2);
            path[0].Should().Be(new Location(1, 1));
            path[1].Should().Be(new Location(2, 1));
        }

        [Fact]
        internal void Finding_a_path_between_two_locations_horizontal_to_each_other()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 1), new Location(4, 1));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(4);
            path[0].Should().Be(new Location(1, 1));
            path[1].Should().Be(new Location(2, 1));
            path[2].Should().Be(new Location(3, 1));
            path[3].Should().Be(new Location(4, 1));
        }

        [Fact]
        internal void Finding_a_path_between_two_locations_vertical_to_each_other()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(4, 1), new Location(4, 4));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(4);
            path[0].Should().Be(new Location(4, 1));
            path[1].Should().Be(new Location(4, 2));
            path[2].Should().Be(new Location(4, 3));
            path[3].Should().Be(new Location(4, 4));
        }

        [Fact]
        internal void Finding_a_moderately_complex_path_between_two_locations_horizontal_to_each_other_and_obstacles_in_between()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(10, 13), new Location(14, 13));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(9);
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
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(2, 3), new Location(4, 3));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(0);
        }

        [Fact]
        internal void Finding_a_path_when_the_start_and_end_are_both_unwalkable()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();

            Path path = sut.FindPath(new Location(1, 3), new Location(3, 3));

            path.Should().NotBeNull();
            path.NodeCount.Should().Be(0);
        }

        [Fact]
        public void Finding_a_path_from_one_location_to_three_locations_returns_three_paths_sorted_by_increasing_number_of_nodes()
        {
            Graph sut = CreateGraphWithNoDiagonalNodesAllowed();
            ImmutableList<Location> locations = ImmutableList.Create(new Location(1, 1), new Location(4, 4), new Location(2, 2));

            ImmutableList<Path> paths = sut.FindPaths(new Location(1, 1), locations);

            paths.Should().NotBeNull();
            paths.Count.Should().Be(3);
            paths[0].NodeCount.Should().BeLessOrEqualTo(paths[1].NodeCount);
            paths[1].NodeCount.Should().BeLessOrEqualTo(paths[2].NodeCount);
        }
    }

    // Factory method to create a graph with diagonal nodes allowed
    private static Graph CreateGraphWithDiagonalNodesAllowed()
    {
        MapFilePath mapFilePath =
            new(
                "../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });

        return map.GetGraph(0, collisionMasks, allowDiagonal: true);
    }

    // Factory method to create a graph with no diagonal nodes allowed
    private static Graph CreateGraphWithNoDiagonalNodesAllowed()
    {
        MapFilePath mapFilePath =
            new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });

        return map.GetGraph(0, collisionMasks, allowDiagonal: false);
    }
}
