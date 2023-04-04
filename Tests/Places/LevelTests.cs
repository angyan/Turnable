using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Places;

public class LevelTests
{
    [Fact]
    internal void Calculating_the_bounds_of_a_map()
    {
        Map sut = new(-1, 16, false, Array.Empty<Layer>(), 1, 1, "orthogonal", "right-down", "1.9.2", 32, Array.Empty<Tileset>(), 32, "map",
            "1.9", 20);

        Bounds bounds = sut.GetBounds();

        bounds.Should().Be(new Bounds(new(0, 0), new(20, 16)));
    }

    [Fact]
    internal void Calculating_the_bounds_of_a_layer()
    {
        Layer sut = new(new int[] {0, 0, 0, 0}, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);

        Bounds bounds = sut.GetBounds();

        bounds.Should().Be(new Bounds(new(0, 0), new(2, 2)));
    }

    [Fact]
    internal void Getting_the_global_tile_id_at_a_particular_location()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();

        int tileAt00 = map.Layers[1].GetTileGid(new Location(0, 0));
        int tileAt10 = map.Layers[1].GetTileGid(new Location(1, 0));
        int tileAt01 = map.Layers[1].GetTileGid(new Location(0, 1));

        tileAt00.Should().Be(948);
        tileAt10.Should().Be(949);
        tileAt01.Should().Be(997);
    }

    [Theory]
    [InlineData(0, 0, 2, 0)]
    [InlineData(1, 1, 2, 3)]
    [InlineData(1, 2, 4, 9)]
    internal void Getting_the_index_in_a_two_dimensional_data_array_that_corresponds_to_a_location(int x, int y, int width, int expectedIndex)
    {
        Location location = new(x, y);
        
        int index = Level.GetDataIndex(location, width);

        index.Should().Be(expectedIndex);
    }

    [Theory]
    [InlineData(1, 0, 0)]
    [InlineData(2, 1, 0)]
    [InlineData(50, 0, 1)]
    [InlineData(51, 1, 1)]
    internal void Getting_the_atlas_location_for_a_global_tile_id_in_a_tileset(int globalTileId, int atlasX, int atlasY)
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        TilesetFilePath tilesetFilePath = new("../../../Fixtures/tileset.tsj");
        TilesetJsonString tilesetJsonString = new(File.ReadAllText(tilesetFilePath));
        Tileset sut = map.Tilesets[0].DeserializeAndMerge(tilesetJsonString);

        Location atlasLocation = sut.GetAtlasLocation(globalTileId);

        atlasLocation.Should().Be(new Location(atlasX, atlasY));
    }

    [Fact]
    internal void All_non_zero_global_tile_ids_are_obstacles_in_a_layer()
    {
        Layer sut = new(new int[] {0, 0, 1, 1}, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);

        ImmutableList<Location> obstacles = sut.GetObstacles();

        obstacles.Count.Should().Be(2); // Any non-zero Global Tile Id is an obstacle
        obstacles.Should().Contain(new Location(1, 1));
        obstacles.Should().NotContain(new Location(0, 0));
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

        ImmutableList<Location> obstacles = sut.GetObstacles(collisionMasks);

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

        Graph graph = sut.GetGraph(0, collisionMasks, allowDiagonal: true);

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

        Graph graph = sut.GetGraph(0, collisionMasks, allowDiagonal: true, additionalObstacles);
        
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

        Graph graph = sut.GetGraph(0, collisionMasks, allowDiagonal: false);

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

        Graph graph = sut.GetGraph(0, collisionMasks, allowDiagonal: false, additionalObstacles);
        
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