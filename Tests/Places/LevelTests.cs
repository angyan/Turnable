using NUnit.Framework;
using Turnable.Places;
using Turnable.Tiled;

namespace Tests.Places
{
    public class LevelTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor()
        {
            // NOTE: NCrunch runs the test process under bin/debug, but the Fixtures folder is three levels above that
            var mapPath = "../../../Fixtures/orthogonal_base64_zlib_left_up_16x16_16x16_with_layer_data_and_external_tileset.tmx";
            var level = new Level(mapPath);

            // Is the Tiled Map loaded up correctly?
            Assert.That(level.Map, Is.Not.Null);
            // Are the tile GlobalIds available via the indexer?
            Assert.That(level[0, 0, 0], Is.EqualTo(948));
        }
    }
}
