using Turnable.AI.Pathfinding;
using FluentAssertions;
using Turnable.Tiled;

namespace Tests.Tiled
{
    public class MapTests
    {
        // NOTE: NCrunch runs the test process under bin/debug,
        // but the Fixtures folder is three levels above that
        // in the "workspace" that NCrunch executes the tests in;
        // this is a minor implementation detail relevant only
        // to tests using files (Fixtures) when using NCrunch

        [Fact]
        public void Loading_an_empty_map()
        {
            // Arrange
            const string mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty.tmx";

            // Act
            Map sut = Map.Load(mapPath);

            // Assert - Basic properties of the map
            sut.Should().NotBeNull();
            sut.Version.Should().Be("1.4");
            sut.TiledVersion.Should().Be("1.4.1");
            sut.Orientation.Should().Be(Orientation.Orthogonal);
            sut.RenderOrder.Should().Be(RenderOrder.LeftUp);
            sut.Width.Should().Be(16);
            sut.Height.Should().Be(16);
            sut.TileWidth.Should().Be(48);
            sut.TileHeight.Should().Be(48);
            sut.Infinite.Should().BeFalse();
            sut.NextLayerId.Should().Be(2);
            sut.NextObjectId.Should().Be(1);

            // Assert - Even an empty map has at least one map layer
            sut.Layers.Should().NotBeNull();
            sut.Layers.Count.Should().Be(1);
            sut.Layers[0].Id.Should().Be(1);
            sut.Layers[0].Name.Should().Be("Tile Layer 1");
            sut.Layers[0].Width.Should().Be(16);
            sut.Layers[0].Height.Should().Be(16);

            // Assert - An empty map still has data with an encoding and compression
            sut.Layers[0].Data.Should().NotBeNull();
            sut.Layers[0].Data.Encoding.Should().Be(Encoding.Base64);
            sut.Layers[0].Data.Compression.Should().Be(Compression.Zlib);
            sut.Layers[0].Data.Value.Should().Be("eJxjYBgFo2AUjFQAAAQAAAE=");
        }

        [Fact]
        public void Loading_an_empty_map_with_map_and_layer_properties()
        {
            // Arrange
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty_with_map_and_layer_properties.tmx";
            
            // Act
            Map sut = Map.Load(mapPath);

            // Assert - Properties on the map
            sut.Properties.Should().NotBeNull();
            sut.Properties.Count.Should().Be(2);
            sut.Properties[0].Name.Should().Be("property_string_1");
            sut.Properties[0].Value.Should().Be("map_1");
            sut.Properties[1].Name.Should().Be("property_string_2");
            sut.Properties[1].Value.Should().Be("map_2");

            // Assert - Properties on the layer
            sut.Layers[0].Properties.Should().NotBeNull();
            sut.Layers[0].Properties.Count.Should().Be(2);
            sut.Layers[0].Properties[0].Name.Should().Be("property_string_1");
            sut.Layers[0].Properties[0].Value.Should().Be("layer_1");
            sut.Layers[0].Properties[1].Name.Should().Be("property_string_2");
            sut.Layers[0].Properties[1].Value.Should().Be("layer_2");
        }

        [Fact]
        public void Loading_an_empty_map_with_an_embedded_tileset()
        {
            // Arrange
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty_with_embedded_tileset.tmx";

            // Act
            Map sut = Map.Load(mapPath);

            // Assert - Each embedded tileset
            sut.Tilesets.Should().NotBeNull();
            sut.Tilesets.Count.Should().Be(1);
            sut.Tilesets[0].FirstGid.Should().Be(1);
            sut.Tilesets[0].Name.Should().Be("embedded_tileset");
            sut.Tilesets[0].TileWidth.Should().Be(16);
            sut.Tilesets[0].TileHeight.Should().Be(16);
            sut.Tilesets[0].TileCount.Should().Be(352);
            sut.Tilesets[0].Columns.Should().Be(16);

            // Assert - The image referenced in the embedded tileset
            sut.Tilesets[0].Image.Should().NotBeNull();
            sut.Tilesets[0].Image.Source.Should().Be("image.png");
            sut.Tilesets[0].Image.Width.Should().Be(256);
            sut.Tilesets[0].Image.Height.Should().Be(352);
        }

        [Fact]
        public void Loading_map_with_external_tileset()
        {
            // Arrange
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_16x16_with_layer_data_and_external_tileset.tmx";

            // Act
            Map sut = Map.Load(mapPath);

            // Assert - Each external tileset
            sut.Tilesets.Should().NotBeNull();
            sut.Tilesets.Count.Should().Be(1);
            sut.Tilesets[0].FirstGid.Should().Be(1);
            sut.Tilesets[0].Source.Should().Be("tileset.tsx");
            sut.Tilesets[0].Name.Should().Be("tileset");
            sut.Tilesets[0].TileWidth.Should().Be(16);
            sut.Tilesets[0].TileHeight.Should().Be(16);
            sut.Tilesets[0].TileCount.Should().Be(1078);
            sut.Tilesets[0].Columns.Should().Be(49);

            // Assert - The image referenced in the external tileset
            sut.Tilesets[0].Image.Should().NotBeNull();
            sut.Tilesets[0].Image.Source.Should().Be("tilesheet.png");
            sut.Tilesets[0].Image.Width.Should().Be(784);
            sut.Tilesets[0].Image.Height.Should().Be(352);
        }

        [Fact]
        public void Loading_map_with_layer_data_encoded_in_base_64_and_compressed_with_zlib()
        {
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_16x16_with_layer_data_and_external_tileset.tmx";

            Map sut = Map.Load(mapPath);

            sut.Layers[0].Data.Should().NotBeNull();
            sut.Layers[0].Data.Encoding.Should().Be(Encoding.Base64);
            sut.Layers[0].Data.Compression.Should().Be(Compression.Zlib);
            sut.Layers[0].Data.Value.Should().Be("eJzbwszAsJVMvA2InwIxueD5qP5R/aP6KdIvxsLAIE4mlgBiABB0Jq0=");
        }

        [Fact]
        public void Loading_map_with_multiple_layers_with_data_encoded_in_base_64_and_compressed_with_zlib()
        {
            // Arrange
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_16x16_with_multiple_layer_data_and_external_tileset.tmx";

            // Act
            Map sut = Map.Load(mapPath);

            // Assert - First layer
            sut.Layers.Should().NotBeNull();
            sut.Layers.Count.Should().Be(2);
            sut.Layers[0].Id.Should().Be(1);
            sut.Layers[0].Name.Should().Be("Background");
            sut.Layers[0].Width.Should().Be(16);
            sut.Layers[0].Height.Should().Be(16);
            sut.Layers[0].Data.Should().NotBeNull();
            sut.Layers[0].Data.Encoding.Should().Be(Encoding.Base64);
            sut.Layers[0].Data.Compression.Should().Be(Compression.Zlib);
            sut.Layers[0].Data.Value.Should().Be("eJxjYKAcMJKJR/WP6h/VT7l+SgAAjZcAxQ==");

            // Assert - Second layer
            sut.Layers[1].Id.Should().Be(3);
            sut.Layers[1].Name.Should().Be("Objects");
            sut.Layers[1].Width.Should().Be(16);
            sut.Layers[1].Height.Should().Be(16);
            sut.Layers[1].Data.Should().NotBeNull();
            sut.Layers[1].Data.Encoding.Should().Be(Encoding.Base64);
            sut.Layers[1].Data.Compression.Should().Be(Compression.Zlib);
            sut.Layers[1].Data.Value.Should().Be("eJzbwszAsBUHtmLALQfC24D4KRDjAla4pcDgOZJ+Ywr0GzMgMCX6ybUfF6C2fvMBtn9UPwNDMgX6k5EwJfYj6xdjYWAQx4FB+nHJgbAEEAMAEMEtNQ==");
        }
    }
}

