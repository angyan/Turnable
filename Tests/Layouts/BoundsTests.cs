using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts
{
    public class BoundsTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public void Bounds_cannot_be_constructed_with_a_length_or_width_less_than_or_equal_to_zero(int width, int height)
        {
            // No arrange
            
            Action construction = () => new Bounds(3, 4, width, height);

            construction.Should().Throw<ArgumentException>().WithMessage($"A width of {width} and a height of {height} are not valid dimensions for a bounds");
        }

        [Theory]
        [InlineData(0, -1)]
        [InlineData(-1, 0)]
        [InlineData(-1, -1)]
        public void Bounds_cannot_be_constructed_with_the_top_left_corner_beyond_the_origin(int x, int y)
        {
            // No arrange
            
            Action construction = () => new Bounds(x, y, 3, 4);

            construction.Should().Throw<ArgumentException>().WithMessage($"{x}, {y} are not valid coordinates for the top left position of a bounds");
        }
    }
}