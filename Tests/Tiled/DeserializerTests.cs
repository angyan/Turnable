using FluentAssertions;
using Turnable.Tiled;
using Turnable.TiledMap;
using Xunit.Abstractions;

namespace Tests.Tiled
{
    public class DeserializerTests
    {
        private readonly ITestOutputHelper _output;

        public DeserializerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void Deserializing_an_empty_map()
        {
            // Arrange
            MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x20_tile_dimensions_32x32_empty.tmj");
            MapJsonString sut = new(File.ReadAllText(mapFilePath.Value));

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
        public void Deserializing_a_map_that_is_not_empty_with_multiple_layers()
        {
            // Arrange
            MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
            MapJsonString sut = new(File.ReadAllText(mapFilePath.Value));

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
        
        [Fact]
        public void Deserializing_an_external_tileset()
        {
            // Arrange
            TilesetFilePath tilesetFilePath = new("../../../Fixtures/tileset.tsj");
            TilesetJsonString sut = new(File.ReadAllText(tilesetFilePath.Value));

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

        public void Deserializing_a_partially_deserialized_external_tileset()
        {
            // Arrange
            MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
            MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath.Value));
            Map map = mapJsonString.Deserialize();


            // Act
            TilesetFilePath tilesetFilePath = new("../../../Fixtures/tileset.tsj");
            TilesetJsonString tilesetJsonString = new(File.ReadAllText(tilesetFilePath.Value));
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

    // NOTE: NCrunch runs the test process under bin/debug,
    // but the Fixtures folder is three levels above that
    // in the "workspace" that NCrunch executes the tests in;
    // this is a minor implementation detail relevant only
    // to tests using files (Fixtures) when using NCrunch
}

