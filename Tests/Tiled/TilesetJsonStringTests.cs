using FluentAssertions;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Tiled;

public class TilesetJsonStringTests
{
    [Fact]
    internal void Json_cannot_be_constructed_with_the_string_null()
    {
        // No arrange

        Action construction = () => new TilesetJsonString("null");

        construction.Should().Throw<ArgumentException>().WithMessage(@"""null"" is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Json_cannot_be_constructed_with_a_value_of_null()
    {
        // No arrange

        Action construction = () => new TilesetJsonString(null);

        construction.Should().Throw<ArgumentException>().WithMessage(@"null is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Tileset_json_string_can_be_implicitly_converted_to_a_string()
    {
        TilesetJsonString tilesetJsonString = new("{}");

        string tilesetJson = tilesetJsonString ;

        tilesetJson.Should().Be("{}");
    }

    [Fact]
    internal void Deserializing_an_external_tileset()
    {
        // Arrange
        TilesetFilePath tilesetFilePath = new("../../../Fixtures/tileset.tsj");
        TilesetJsonString sut = new(File.ReadAllText(tilesetFilePath));

        // Act
        Tileset tileset = sut.Deserialize();

        // Assert - External tileset
        tileset.Should().NotBeNull();
        tileset.Columns.Should().Be(49);
        tileset.Image.Should().Be("tilesheet.png");
        tileset.ImageHeight.Should().Be(352);
        tileset.ImageWidth.Should().Be(784);
        tileset.Margin.Should().Be(0);
        tileset.Name.Should().Be("tileset");
        tileset.Spacing.Should().Be(0);
        tileset.TileCount.Should().Be(1078);
        tileset.TiledVersion.Should().Be("1.9.2");
        tileset.TileHeight.Should().Be(16);
        tileset.TileWidth.Should().Be(16);
        tileset.Type.Should().Be("tileset");
        tileset.Version.Should().Be("1.9");
    }

    [Fact]

    internal void Deserializing_a_partially_deserialized_external_tileset()
    {
        // Arrange
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();


        // Act
        TilesetFilePath tilesetFilePath = new("../../../Fixtures/tileset.tsj");
        TilesetJsonString tilesetJsonString = new(File.ReadAllText(tilesetFilePath));
        Tileset tileset = map.Tilesets[0];
        tileset = tileset.DeserializeAndMerge(tilesetJsonString);

        // Assert - Merged tileset
        tileset.Should().NotBeNull();
        tileset.Columns.Should().Be(49);
        tileset.FirstGid.Should().Be(1);
        tileset.Image.Should().Be("tilesheet.png");
        tileset.ImageHeight.Should().Be(352);
        tileset.ImageWidth.Should().Be(784);
        tileset.Margin.Should().Be(0);
        tileset.Name.Should().Be("tileset");
        tileset.Source.Should().Be("tileset.tsj");
        tileset.Spacing.Should().Be(0);
        tileset.TileCount.Should().Be(1078);
        tileset.TiledVersion.Should().Be("1.9.2");
        tileset.TileHeight.Should().Be(16);
        tileset.TileWidth.Should().Be(16);
        tileset.Type.Should().Be("tileset");
        tileset.Version.Should().Be("1.9");
    }
}