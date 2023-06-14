using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts;

public class LocationTests
{
    [Theory]
    [InlineData(1, 1, 3, 1, 2)]
    [InlineData(1, 1, 1, 3, 2)]
    [InlineData(1, 1, 3, 3, 4)]
    [InlineData(1, 1, 2, 2, 2)]
    [InlineData(1, 1, 1, 1, 0)]
    internal void Distance_between_two_locations_without_diagonal_movement_is_the_manhattan_distance(int startX, int startY, int endX, int endY, int expectedDistance)
    {
        Location start = new(startX, startY);
        Location end = new(endX, endY);

        int distance = start.DistanceTo(end, allowDiagonal: false);

        distance.Should().Be(expectedDistance);
    }


    [Theory]
    [InlineData(1, 1, 3, 1, 2)]
    [InlineData(1, 1, 2, 3, 2)]
    [InlineData(1, 1, 3, 3, 2)]
    [InlineData(1, 1, 4, 2, 3)]
    [InlineData(1, 1, 1, 1, 0)]
    internal void Distance_between_two_locations_with_diagonal_movement_allowed_is_the_euclidean_distance_in_steps(
        int startX, int startY, int endX, int endY, int expectedDistance)
    {
        Location start = new(startX, startY);
        Location end = new(endX, endY);

        int distance = start.DistanceTo(end, allowDiagonal: true);

        distance.Should().Be(expectedDistance);
    }

}
