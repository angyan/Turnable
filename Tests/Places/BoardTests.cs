using NUnit.Framework;
using Turnable.Places;

namespace Tests.Places
{
    public class BoardTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ConstructorWithLength()
        {
            var Board = new Board(10);

            Assert.That(Board.Bounds, Is.Not.Null);
            Assert.That(Board.Bounds.Width, Is.EqualTo(10));
            Assert.That(Board.Bounds.Height, Is.EqualTo(10));
        }

        [Test]
        public void ConstructorWithWidthAndHeight()
        {
            var Board = new Board(10, 12);

            Assert.That(Board.Bounds, Is.Not.Null);
            Assert.That(Board.Bounds.Width, Is.EqualTo(10));
            Assert.That(Board.Bounds.Height, Is.EqualTo(12));
        }
    }
}
