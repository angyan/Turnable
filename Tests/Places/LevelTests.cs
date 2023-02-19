using System.Collections.Immutable;
using FluentAssertions;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Places;

public class LevelTests
{
    [Fact]
    public void Calculating_the_bounds_of_a_map()
    {
        Map sut = new(-1, 16, false, Array.Empty<Layer>(), 1, 1, "orthogonal", "right-down", "1.9.2", 32, Array.Empty<Tileset>(), 32, "map",
            "1.9", 20);

        Bounds bounds = sut.GetBounds();

        bounds.Should().Be(new Bounds(new Location(0, 0), 20, 16));
    }

    [Fact]
    public void Calculating_the_bounds_of_a_layer()
    {
        Layer sut = new(new int[] {0, 0, 0, 0}, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);

        Bounds bounds = sut.GetBounds();

        bounds.Should().Be(new Bounds(new Location(0, 0), 2, 2));
    }

    [Fact]
    public void Converting_a_layer_to_individual_obstacles()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        Layer sut = map.Layers[1];

        ImmutableList<Location> obstacles = sut.GetObstacles();

        obstacles.Count.Should().Be(87); // Any non-zero Global Tile Id is an obstacle
        obstacles.Should().Contain(new Location(0, 0));
        obstacles.Should().NotContain(new Location(2, 1));
    }

    [Fact]
    public void Getting_the_graph_for_a_layer()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        Layer sut = map.Layers[1];

        ImmutableDictionary<Location, ImmutableList<Location>> graph = sut.GetGraph(ImmutableList.Create<Layer>(map.Layers[1]));

        graph.Count.Should().Be(256); // Each location in the layer is a possible node (even if it's not walkable)
        // Based on how this map is set up, each corner of the lowermost layer should have just 1 walkable neighbor
        graph[new Location(0, 0)].Count.Should().Be(1);
        graph[new Location(15, 0)].Count.Should().Be(1);
        graph[new Location(0, 15)].Count.Should().Be(1);
        graph[new Location(15, 15)].Count.Should().Be(1);
        // Based on how this map is set up, test how many neighbors a few other nodes have
        graph[new Location(1, 14)].Count.Should().Be(3);
        graph[new Location(2, 3)].Count.Should().Be(3);
        graph[new Location(3, 4)].Count.Should().Be(5);
    }
}