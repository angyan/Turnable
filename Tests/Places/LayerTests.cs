using System.Collections.Immutable;
using FluentAssertions;
using Turnable.Layouts;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Places;

public class LayerTests
{
    [Fact]
    internal void The_bounds_of_a_layer_are_based_on_its_location_width_and_height()
    {
        Layer sut = new(new int[] {0, 0, 0, 0}, 2, 1, "Layer", 1, "layer", true, 2, 0, 0);

        Bounds bounds = sut.Bounds;

        bounds.Should().Be(new Bounds(new(0, 0), new(2, 2)));
    }

    [Fact]
    internal void The_global_tile_id_at_a_location_is_the_correct_value_from_the_layers_data()
    {
        Map map = Map.Load("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        Layer sut = map.Layers[1];

        int tileAt00 = sut.TileGid(new Location(0, 0));
        int tileAt10 = sut.TileGid(new Location(1, 0));
        int tileAt01 = sut.TileGid(new Location(0, 1));

        tileAt00.Should().Be(948);
        tileAt10.Should().Be(949);
        tileAt01.Should().Be(997);
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
}