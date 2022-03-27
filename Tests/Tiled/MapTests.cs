using NUnit.Framework;
using Turnable.Tiled;

namespace Tests.Tiled
{
    public class MapTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void LoadEmptyMap()
        {
            // NOTE: NCrunch runs the test process under bin/debug, but the Fixtures folder is three levels above that
            var mapPath = ("../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty.tmx");
            var map = Map.Load(mapPath);

            // Are the map attributes correctly set?
            Assert.That(map, Is.Not.Null);
            Assert.That(map.Version, Is.EqualTo("1.4"));
            Assert.That(map.TiledVersion, Is.EqualTo("1.4.1"));
            Assert.That(map.Orientation, Is.EqualTo(Orientation.Orthogonal));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.LeftUp));
            Assert.That(map.Width, Is.EqualTo(16));
            Assert.That(map.Height, Is.EqualTo(16));
            Assert.That(map.TileWidth, Is.EqualTo(48));
            Assert.That(map.TileHeight, Is.EqualTo(48));
            Assert.That(map.Infinite, Is.False);
            Assert.That(map.NextLayerId, Is.EqualTo(2));
            Assert.That(map.NextObjectId, Is.EqualTo(1));

            // Are the layers correctly loaded?
            Assert.That(map.Layers, Is.Not.Null);
            Assert.That(map.Layers.Count, Is.EqualTo(1));
            Layer layer = map.Layers[0];
            Assert.That(layer.Id, Is.EqualTo(1));
            Assert.That(layer.Name, Is.EqualTo("Tile Layer 1"));
            Assert.That(layer.Width, Is.EqualTo(16));
            Assert.That(layer.Height, Is.EqualTo(16));

            // Was the data for the layer correctly loaded?
            Assert.That(layer.Data, Is.Not.Null);
            Assert.That(layer.Data.Encoding, Is.EqualTo(Encoding.Base64));
            Assert.That(layer.Data.Compression, Is.EqualTo(Compression.Zlib));
            Assert.That(layer.Data.Value, Is.EqualTo("eJxjYBgFo2AUjFQAAAQAAAE="));
        }

        [Test]
        public void LoadEmptyMapWithMapAndLayerProperties()
        {
            // NOTE: NCrunch runs the test process under bin/debug, but the Fixtures folder is three levels above that
            var mapPath = ("../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty_with_map_and_layer_properties.tmx");
            var map = Map.Load(mapPath);

            // Are the map properties correctly loaded?
            // TODO: The library currently only supports properties of stype string
            Assert.That(map.Properties, Is.Not.Null);
            Assert.That(map.Properties.Count, Is.EqualTo(2));
            Property property1 = map.Properties[0];
            Property property2 = map.Properties[1];
            Assert.That(property1.Name, Is.EqualTo("property_string_1"));
            Assert.That(property1.Value, Is.EqualTo("map_1"));
            Assert.That(property2.Name, Is.EqualTo("property_string_2"));
            Assert.That(property2.Value, Is.EqualTo("map_2"));

            // Are the layer properties correctly loaded?
            // TODO: The library currently only supports properties of stype string
            Layer layer = map.Layers[0];

            Assert.That(layer.Properties, Is.Not.Null);
            Assert.That(layer.Properties.Count, Is.EqualTo(2));
            property1 = layer.Properties[0];
            property2 = layer.Properties[1];
            Assert.That(property1.Name, Is.EqualTo("property_string_1"));
            Assert.That(property1.Value, Is.EqualTo("layer_1"));
            Assert.That(property2.Name, Is.EqualTo("property_string_2"));
            Assert.That(property2.Value, Is.EqualTo("layer_2"));
        }

        [Test]
        public void LoadEmptyMapWithEmbeddedTileset()
        {
            // NOTE: NCrunch runs the test process under bin/debug, but the Fixtures folder is three levels above that
            var mapPath = ("../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_48x48_empty_with_embedded_tileset.tmx");
            var map = Map.Load(mapPath);

            // Are the tilesets correctly loaded?
            Assert.That(map.Tilesets, Is.Not.Null);
            Assert.That(map.Tilesets.Count, Is.EqualTo(1));
            var tileset = map.Tilesets[0];
            Assert.That(tileset.FirstGid, Is.EqualTo(1));
            Assert.That(tileset.Name, Is.EqualTo("embedded_tileset"));
            Assert.That(tileset.TileWidth, Is.EqualTo(16));
            Assert.That(tileset.TileWidth, Is.EqualTo(16));
            Assert.That(tileset.TileCount, Is.EqualTo(352));
            Assert.That(tileset.Columns, Is.EqualTo(16));

            // Is the image of the tileset correctly loaded?
            Assert.That(tileset.Image, Is.Not.Null);
            var image = tileset.Image;
            Assert.That(image.Source, Is.EqualTo("image.png"));
            Assert.That(image.Width, Is.EqualTo(256));
            Assert.That(image.Height, Is.EqualTo(352));
        }
    }
}
