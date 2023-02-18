using FluentAssertions;
using Turnable.Layouts;
using Xunit.Abstractions;

namespace Tests.Layouts
{
    public class GridTests
    {
        private readonly ITestOutputHelper _output;

        public GridTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        // Locations at corners of bounds
        [InlineData(3, 4)]
        [InlineData(12, 4)]
        [InlineData(12, 8)]
        [InlineData(3, 8)]
        // Locations along edges
        [InlineData(4, 4)]
        [InlineData(12, 5)]
        [InlineData(11, 8)]
        [InlineData(3, 7)]
        // Locations inside bounds
        [InlineData(5, 6)]
        public void Bounds_contains_x_y_coordinates_within_itself(int x, int y)
        {
            Bounds sut = new(3, 4, 10, 5);

            bool contains = sut.Contains(x, y);

            contains.Should().BeTrue();
        }

        [Theory]
        [InlineData(2, 5)]
        [InlineData(8, 1)]
        [InlineData(16, 6)]
        [InlineData(5, 10)]
        public void Bounds_does_not_contain_x_y_coordinates_outside_itself(int x, int y)
        {
            Bounds sut = new(3, 4, 10, 5);

            bool contains = sut.Contains(x, y);

            contains.Should().BeFalse();
        }

        [Theory]
        // Locations at corners of bounds
        [InlineData(3, 4)]
        [InlineData(12, 4)]
        [InlineData(12, 8)]
        [InlineData(3, 8)]
        // Locations along edges
        [InlineData(4, 4)]
        [InlineData(12, 5)]
        [InlineData(11, 8)]
        [InlineData(3, 7)]
        // Locations inside bounds
        [InlineData(5, 6)]
        internal void Bounds_contains_locations_within_itself(int x, int y)
        {
            Bounds sut = new(3, 4, 10, 5);
            Location location = new(x, y);

            bool contains = sut.Contains(location);

            contains.Should().BeTrue();
        }

        [Theory]
        [InlineData(2, 5)]
        [InlineData(8, 1)]
        [InlineData(16, 6)]
        [InlineData(5, 10)]
        internal void Bounds_does_not_contain_locations_outside_itself(int x, int y)
        {
            Bounds sut = new(3, 4, 10, 5);
            Location location = new(x, y);

            bool contains = sut.Contains(location);

            contains.Should().BeFalse();
        }

        [Theory]
        [MemberData(nameof(AllNeighbors))]
        internal void Getting_all_possible_neighbors_for_a_grid_location(GridLocation location, List<Location> expectedNeighbors)
        {
            Bounds sut = new(3, 4, 10, 5);

            List<Location> neighbors = sut.NeighborsFor(location);

            neighbors.Should().BeEquivalentTo(expectedNeighbors);
        }

        [Theory]
        [MemberData(nameof(Neighbors))]
        internal void Excluding_neighbors_for_a_grid_location_that_fall_out_of_some_bounds(GridLocation gridLocation, List<Location> expectedNeighbors)
        {
            Bounds sut = new(3, 4, 10, 5);

            List<Location> neighbors = sut.NeighborsFor(gridLocation, (bounds, location) => bounds.Contains(location));

            neighbors.Should().BeEquivalentTo(expectedNeighbors);
        }

        private static IEnumerable<object[]> AllNeighbors()
        {
            Bounds bounds = new(3, 4, 10, 5);

            // Neighbors for top left corner
            yield return new object[]
            {
                new GridLocation(3, 4, bounds),
                new List<Location>
                {
                    new Location(2, 4),
                    new Location(2, 3),
                    new Location(3, 3),
                    new Location(4, 3),
                    new Location(4, 4),
                    new Location(4, 5),
                    new Location(3, 5),
                    new Location(2, 5),
                }
            };

            // Neighbors for a location away from all edges
            yield return new object[]
            {
                new GridLocation(4, 5, bounds),
                new List<Location>
                {
                    new Location(3, 4),
                    new Location(4, 4),
                    new Location(5, 4),
                    new Location(5, 5),
                    new Location(5, 6),
                    new Location(4, 6),
                    new Location(3, 6),
                    new Location(3, 5),
                }
            };
        }
        private static IEnumerable<object[]> Neighbors()
        {
            Bounds bounds = new(3, 4, 10, 5);

            // Neighbors for top left corner
            yield return new object[]
            {
                new GridLocation(3, 4, bounds),
                new List<Location>
                {
                    new Location(4, 4),
                    new Location(4, 5),
                    new Location(3, 5),
                }
            };

            // Neighbors for top right corner
            yield return new object[]
            {
                new GridLocation(12, 4, bounds),
                new List<Location>
                {
                    new Location(11, 4),
                    new Location(11, 5),
                    new Location(12, 5),
                }
            };

            // Neighbors for bottom right corner
            yield return new object[]
            {
                new GridLocation(12, 8, bounds),
                new List<Location>
                {
                    new Location(11, 8),
                    new Location(11, 7),
                    new Location(12, 7),
                }
            };

            // Neighbors for bottom left corner
            yield return new object[]
            {
                new GridLocation(3, 8, bounds),
                new List<Location>
                {
                    new Location(4, 8),
                    new Location(3, 7),
                    new Location(4, 7),
                }
            };

            // Neighbors for a location away from all edges
            yield return new object[]
            {
                new GridLocation(4, 5, bounds),
                new List<Location>
                {
                    new Location(3, 4),
                    new Location(4, 4),
                    new Location(5, 4),
                    new Location(5, 5),
                    new Location(5, 6),
                    new Location(4, 6),
                    new Location(3, 6),
                    new Location(3, 5),
                }
            };


        }
    }
}