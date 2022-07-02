using NUnit.Framework;
using Turnable.Tiled;

namespace Tests.Tiled
{
    public class TilesTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructorWithBase64ZlibData()
        {
            var data = new Data();
            data.Encoding = Encoding.Base64;
            data.Compression = Compression.Zlib;
            data.Value = "eJzbwszAsJVMvA2InwIxueD5qP5R/aP6KdIvxsLAIE4mlgBiABB0Jq0=";

            var tiles = new Tiles(data);
            Assert.IsNotNull(tiles);
            Assert.That(tiles.At(0, 0), Is.EqualTo(948));
        }
    }
}
