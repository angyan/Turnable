using FluentAssertions;
using Turnable.Layouts;
using Turnable.TiledMap;
using Turnable.Tiled;

namespace Tests.Tiled;

public class LayerDataTests
{
    [Fact]
    internal void Getting_the_global_tile_id_at_a_particular_location()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();

        int tileAt00 = map.Layers[1].TileGidAt(new Location(0, 0));
        int tileAt10 = map.Layers[1].TileGidAt(new Location(1, 0));
        int tileAt01 = map.Layers[1].TileGidAt(new Location(0, 1));

        tileAt00.Should().Be(948);
        tileAt10.Should().Be(949);
        tileAt01.Should().Be(997);
    }
}