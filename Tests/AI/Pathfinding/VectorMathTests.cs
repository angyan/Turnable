using Turnable.AI.Pathfinding;
using FluentAssertions;

namespace Tests.AI.Pathfinding
{
    public class VectorMathTests
    {
        [Fact]
        public void Sum_of_two_vectors()
        {
            var vector1 = new Vector2(1, 2);
            var vector2 = new Vector2(2, 3);

            Vector2 result = VectorMath.Add(vector1, vector2);

            result.Should().Be(new Vector2(3, 5));
        }

        [Theory]
        [InlineData(1, 2, -1, -2)]
        [InlineData(-1, -2, 1, 2)]
        public void Negative_of_a_vector(int x, int y, int negatedX, int negatedY)
        {
            var vector = new Vector2(x, y);

            Vector2 result = VectorMath.Negate(vector);

            result.Should().Be(new Vector2(negatedX, negatedY));
        }

        [Fact]
        public void Difference_of_two_vectors()
        {
            var vector1 = new Vector2(3, 5);
            var vector2 = new Vector2(1, 2);

            Vector2 result = VectorMath.Subtract(vector1, vector2);

            result.Should().Be(new Vector2(2, 3));
        }

        [Fact]
        public void Magnitude_of_a_vector()
        {
            var vector = new Vector2(3, 4);

            double result = VectorMath.Magnitude(vector);

            result.Should().Be(5.0);
        }

        [Fact]
        public void Distance_between_two_vectors()
        {
            var vector1 = new Vector2(4, 6);
            var vector2 = new Vector2(1, 2);

            double result = VectorMath.Distance(vector1, vector2);

            result.Should().Be(5.0);
        }
    }
}

