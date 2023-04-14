using FluentAssertions;
using Turnable.Tiled;
using Turnable.TiledMap;

namespace Tests.Tiled;

public class MapJsonStringTests
{
    [Fact]
    internal void Map_json_string_cannot_be_constructed_with_the_string_null()
    {
        // No arrange

        Action construction = () => new MapJsonString("null");

        construction.Should().Throw<ArgumentException>().WithMessage(@"""null"" is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Map_json_string_cannot_be_constructed_with_a_value_of_null()
    {
        // No arrange

        Action construction = () => new MapJsonString(null);

        construction.Should().Throw<ArgumentException>().WithMessage(@"null is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Map_json_string_can_be_implicitly_converted_to_a_string()
    {
        MapJsonString mapJsonString = new("{}");

        string mapJson = mapJsonString;

        mapJson.Should().Be("{}");
    }

    [Fact]
    internal void Deserializing_an_empty_map()
    {
        // Arrange
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x20_tile_dimensions_32x32_empty.tmj");
        MapJsonString sut = new(File.ReadAllText(mapFilePath));

        // Act
        Map map = sut.Deserialize();

        // Assert - Basic properties of the map
        map.Should().NotBeNull();
        map.CompressionLevel.Should().Be(-1);
        map.Height.Should().Be(20);
        map.Infinite.Should().BeFalse();
        map.NextLayerId.Should().Be(2);
        map.NextObjectId.Should().Be(1);
        map.Orientation.Should().Be("orthogonal");
        map.RenderOrder.Should().Be("right-down");
        map.TiledVersion.Should().Be("1.9.2");
        map.TileHeight.Should().Be(32);
        map.Tilesets.Should().NotBeNull();
        map.TileWidth.Should().Be(32);
        map.Type.Should().Be("map");
        map.Version.Should().Be("1.9");
        map.Width.Should().Be(16);

        // Assert - Each layer
        map.Layers.Should().NotBeNull();
        map.Layers.Length.Should().Be(1);
        map.Layers[0].Data.Should().NotBeNull();
        map.Layers[0].Data.Length.Should().Be(20 * 16);
        map.Layers[0].Height.Should().Be(20);
        map.Layers[0].Id.Should().Be(1);
        map.Layers[0].Name.Should().Be("Tile Layer 1");
        map.Layers[0].Opacity.Should().Be(1);
        map.Layers[0].Type.Should().Be("tilelayer");
        map.Layers[0].Visible.Should().BeTrue();
        map.Layers[0].Width.Should().Be(16);
        map.Layers[0].X.Should().Be(0);
        map.Layers[0].Y.Should().Be(0);
    }

    [Fact]
    internal void Deserializing_a_map_that_is_not_empty_with_multiple_layers()
    {
        // Arrange
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString sut = new(File.ReadAllText(mapFilePath));

        // Act
        Map map = sut.Deserialize();

        // Assert - Basic properties of the map
        map.Should().NotBeNull();
        map.CompressionLevel.Should().Be(-1);
        map.Height.Should().Be(16);
        map.Infinite.Should().BeFalse();
        map.NextLayerId.Should().Be(4);
        map.NextObjectId.Should().Be(1);
        map.Orientation.Should().Be("orthogonal");
        map.RenderOrder.Should().Be("right-down");
        map.TiledVersion.Should().Be("1.9.2");
        map.TileHeight.Should().Be(16);
        map.TileWidth.Should().Be(16);
        map.Type.Should().Be("map");
        map.Version.Should().Be("1.9");
        map.Width.Should().Be(16);

        // Assert - First layer
        map.Layers.Should().NotBeNull();
        map.Layers.Length.Should().Be(2);
        map.Layers[0].Data.Should().NotBeNull();
        map.Layers[0].Data.Length.Should().Be(16 * 16);
        map.Layers[0].Data[17].Should().Be(1);
        map.Layers[0].Height.Should().Be(16);
        map.Layers[0].Id.Should().Be(1);
        map.Layers[0].Name.Should().Be("Background");
        map.Layers[0].Opacity.Should().Be(1);
        map.Layers[0].Type.Should().Be("tilelayer");
        map.Layers[0].Visible.Should().BeTrue();
        map.Layers[0].Width.Should().Be(16);
        map.Layers[0].X.Should().Be(0);
        map.Layers[0].Y.Should().Be(0);

        //Assert - Second layer
        map.Layers[1].Data.Should().NotBeNull();
        map.Layers[1].Data.Length.Should().Be(16 * 16);
        map.Layers[1].Data[0].Should().Be(948);
        map.Layers[1].Height.Should().Be(16);
        map.Layers[1].Id.Should().Be(3);
        map.Layers[1].Name.Should().Be("Objects");
        map.Layers[1].Opacity.Should().Be(1);
        map.Layers[1].Type.Should().Be("tilelayer");
        map.Layers[1].Visible.Should().BeTrue();
        map.Layers[1].Width.Should().Be(16);
        map.Layers[1].X.Should().Be(0);
        map.Layers[1].Y.Should().Be(0);

        // Assert - Tileset
        map.Tilesets.Should().NotBeNull();
        map.Tilesets.Length.Should().Be(1);
        map.Tilesets[0].FirstGid.Should().Be(1);
        map.Tilesets[0].Source.Should().Be("tileset.tsj");
    }
}