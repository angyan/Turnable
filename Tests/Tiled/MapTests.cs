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

            Assert.That(map, Is.Not.Null);
            Assert.That(map.Version, Is.EqualTo("1.4"));
            Assert.That(map.TiledVersion, Is.EqualTo("1.4.1"));
            Assert.That(map.Orientation, Is.EqualTo(MapOrientation.Orthogonal));
            Assert.That(map.RenderOrder, Is.EqualTo(RenderOrder.LeftUp));
            Assert.That(map.Width, Is.EqualTo(16));
            Assert.That(map.Height, Is.EqualTo(16));
            Assert.That(map.TileWidth, Is.EqualTo(48));
            Assert.That(map.TileHeight, Is.EqualTo(48));
            Assert.That(map.Infinite, Is.False);
            Assert.That(map.NextLayerId, Is.EqualTo(2));
            Assert.That(map.NextObjectId, Is.EqualTo(1));
        }
    }
}
