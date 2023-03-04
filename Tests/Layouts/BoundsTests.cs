using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts;

public class BoundsTests
{
    [Theory]
    [InlineData(0, -1)]
    [InlineData(-1, 0)]
    [InlineData(-1, -1)]
    internal void Bounds_cannot_be_constructed_with_the_top_left_corner_beyond_the_origin(int x, int y)
    {
        // No arrange
        
        Action construction = () => new Bounds(new(x, y), new(3, 4));

        construction.Should().Throw<ArgumentException>().WithMessage($"{new Location(x, y)} is not a valid Location for a Bounds");
    }
}
