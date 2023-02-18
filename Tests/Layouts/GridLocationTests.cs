using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts
{
    public class GridLocationTests
    {
        [Fact]
        public void Grid_location_cannot_be_constructed_at_x_y_coordinates_outside_a_given_bounds()
        {
            Bounds bounds = new(3, 4, 10, 5);

            Action construction = () => new GridLocation(1, 2, bounds);

            construction.Should().Throw<ArgumentException>().WithMessage($"{new Location(1, 2)} is not a valid location within {bounds}");
        }

        [Fact]
        public void Grid_location_cannot_be_constructed_at_a_location_outside_a_given_bounds()
        {
            Bounds bounds = new(3, 4, 10, 5);

            Action construction = () => new GridLocation(new Location(1, 2), bounds);

            construction.Should().Throw<ArgumentException>().WithMessage($"{new Location(1, 2)} is not a valid location within {bounds}");
        }
    }
}