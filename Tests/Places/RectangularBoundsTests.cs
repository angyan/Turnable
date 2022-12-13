using NUnit.Framework;
using Turnable.Places;
using Turnable.Tiled;

namespace Tests.Places
{
    public class RectanglularBoundsTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Constructor()
        {
            IBounds bounds = new RectangularBounds(10, 12);

            Assert.That(bounds.Width, Is.EqualTo(10));
            Assert.That(bounds.Height, Is.EqualTo(12));
        }
    }
}
