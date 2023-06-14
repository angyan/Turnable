using FluentAssertions;
using Turnable.Layouts;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Tiled;

public class TilesetTests
{
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

        Location atlasLocation = sut.AtlasLocation(globalTileId);

        atlasLocation.Should().Be(new Location(atlasX, atlasY));
    }
}