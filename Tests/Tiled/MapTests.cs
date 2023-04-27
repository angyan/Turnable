using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Tiled;

public class MapTests
{
    [Fact]
    internal void The_bounds_of_a_map_is_based_on_its_location_width_and_height()
    {
        Map sut = new(-1, 16, false, Array.Empty<Layer>(), 1, 1, "orthogonal", "right-down", "1.9.2", 32, Array.Empty<Tileset>(), 32, "map",
            "1.9", 20);

        Bounds bounds = sut.Bounds;

        bounds.Should().Be(new Bounds(new(0, 0), new(20, 16)));
    }

    [Fact]
    internal void All_non_zero_global_tile_ids_in_all_layer_indices_of_a_collision_mask_are_obstacles()
    {
        Layer[] layers = new Layer[3];
        layers[0] = new(new[] { 0, 0, 0, 0 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        layers[1] = new(new[] { 0, 1, 0, 0 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        layers[2] = new(new[] { 0, 0, 0, 1 }, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);
        Map sut = new(-1, 16, false, layers, 1, 1, "orthogonal", "right-down", "1.9.2", 32, Array.Empty<Tileset>(), 32, "map",
            "1.9", 20);
        CollisionMasks collisionMasks = new(sut, 0, new[] {1, 2});

        ImmutableList<Location> obstacles = sut.Obstacles(collisionMasks);

        obstacles.Count.Should().Be(2); // Any non-zero Global Tile Id is an obstacle
        obstacles.Should().Contain(new Location(1, 0));
        obstacles.Should().Contain(new Location(1, 1));
        obstacles.Should().NotContain(new Location(0, 0));
        obstacles.Should().NotContain(new Location(0, 1));
    }

    [Fact]
    internal void Getting_the_graph_for_a_layer_including_diagonal_neighbors()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map sut = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });

        Graph graph = sut.Graph(0, collisionMasks, allowDiagonal: true);

        graph.Count.Should().Be(256); // Each location in the layer is a possible node (even if it's not walkable)
        // Based on how this map is set up, each corner of the lowermost layer should have just 1 walkable neighbor
        graph[new Location(0, 0)].Count.Should().Be(1);
        graph[new Location(15, 0)].Count.Should().Be(1);
        graph[new Location(0, 15)].Count.Should().Be(1);
        graph[new Location(15, 15)].Count.Should().Be(1);
        // Based on how this map is set up, test how many neighbors a few other nodes have
        graph[new Location(1, 14)].Count.Should().Be(3);
        graph[new Location(2, 3)].Count.Should().Be(0);
        graph[new Location(3, 4)].Count.Should().Be(6);
    }

    [Fact]
    internal void Getting_the_graph_for_a_layer_including_diagonal_neighbors_and_including_additional_obstacles()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map sut = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });
        List<Location> additionalObstacles = new List<Location>() { new Location(2, 5), new Location(4, 5) };

        Graph graph = sut.Graph(0, collisionMasks, allowDiagonal: true, additionalObstacles);
        
        graph.Count.Should().Be(256);
        graph[new Location(0, 0)].Count.Should().Be(1);
        graph[new Location(15, 0)].Count.Should().Be(1);
        graph[new Location(0, 15)].Count.Should().Be(1);
        graph[new Location(15, 15)].Count.Should().Be(1);
        graph[new Location(1, 14)].Count.Should().Be(3);
        graph[new Location(2, 3)].Count.Should().Be(0);
        // Two nodes that were empty in the map are now obstacles
        graph[new Location(3, 4)].Count.Should().Be(4);
    }

    [Fact]
    internal void Getting_the_graph_for_a_layer_excluding_non_diagonal_neighbors()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map sut = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });

        Graph graph = sut.Graph(0, collisionMasks, allowDiagonal: false);

        graph.Count.Should().Be(256); // Each location in the layer is a possible node (even if it's not walkable)
        // Based on how this map is set up, each corner of the lowermost layer should have just 1 walkable neighbor
        graph[new Location(0, 0)].Count.Should().Be(0);
        graph[new Location(15, 0)].Count.Should().Be(0);
        graph[new Location(0, 15)].Count.Should().Be(0);
        graph[new Location(15, 15)].Count.Should().Be(0);
        // Based on how this map is set up, test how many neighbors a few other nodes have
        graph[new Location(1, 14)].Count.Should().Be(2);
        graph[new Location(2, 3)].Count.Should().Be(0);
        graph[new Location(3, 4)].Count.Should().Be(2);
    }

    [Fact]
    internal void Getting_the_graph_for_a_layer_excluding_diagonal_neighbors_and_including_additional_obstacles()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map sut = mapJsonString.Deserialize();
        CollisionMasks collisionMasks = new CollisionMasks(new[] { 1 });
        List<Location> additionalObstacles = new List<Location>() { new Location(4, 4) };

        Graph graph = sut.Graph(0, collisionMasks, allowDiagonal: false, additionalObstacles);
        
        graph.Count.Should().Be(256);
        graph[new Location(0, 0)].Count.Should().Be(0);
        graph[new Location(15, 0)].Count.Should().Be(0);
        graph[new Location(0, 15)].Count.Should().Be(0);
        graph[new Location(15, 15)].Count.Should().Be(0);
        // One node that was empty in the map is now an obstacle
        graph[new Location(1, 14)].Count.Should().Be(2);
        graph[new Location(2, 3)].Count.Should().Be(0);
        graph[new Location(3, 4)].Count.Should().Be(1);
    }
}