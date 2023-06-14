using FluentAssertions;
using System.Collections.Immutable;
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
        Bounds sut = new(new Location(3, 4), new Size(10, 5));
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
        Bounds sut = new(new Location(3, 4), new Size(10, 5));
        Location location = new(x, y);

        bool contains = sut.Contains(location);

        contains.Should().BeFalse();
    }

    public class GettingNeighborsTests
    {
        [Theory]
        [MemberData(nameof(ContainedNeighbors))]
        internal void Getting_neighbors_contained_within_some_bounds(Location location,
            ImmutableList<Location> expectedNeighbors)
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 5));

            ImmutableList<Location> neighbors = Bounds.GetNeighbors(sut, location, allowDiagonal: true, _ => true);

            neighbors.Should().BeEquivalentTo(expectedNeighbors);
        }

        [Theory]
        [MemberData(nameof(ContainedNonDiagonalNeighbors))]
        internal void Getting_only_non_diagonal_neighbors_contained_within_some_bounds(Location location,
            ImmutableList<Location> expectedNeighbors)
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 5));

            ImmutableList<Location> neighbors = Bounds.GetNeighbors(sut, location, allowDiagonal: false, _ => true);

            neighbors.Should().BeEquivalentTo(expectedNeighbors);
        }

        private static IEnumerable<object[]> ContainedNeighbors()
        {
            Bounds bounds = new(new(3, 4), new(10, 5));

            // Neighbors for top left corner
            yield return new object[]
            {
                new Location(3, 4),
                ImmutableList.Create<Location>
                (
                    new(4, 4),
                    new(4, 5),
                    new(3, 5)
                )
            };

            // Neighbors for top right corner
            yield return new object[]
            {
                new Location(12, 4),
                ImmutableList.Create<Location>
                (
                    new(11, 4),
                    new(11, 5),
                    new(12, 5)
                )
            };

            // Neighbors for bottom right corner
            yield return new object[]
            {
                new Location(12, 8),
                ImmutableList.Create<Location>
                (
                    new(11, 8),
                    new(11, 7),
                    new(12, 7)
                )
            };

            // Neighbors for bottom left corner
            yield return new object[]
            {
                new Location(3, 8),
                ImmutableList.Create<Location>
                (
                    new(4, 8),
                    new(3, 7),
                    new(4, 7)
                )
            };

            // Neighbors for a location away from all edges
            yield return new object[]
            {
                new Location(4, 5),
                ImmutableList.Create<Location>
                (
                    new(3, 4),
                    new(4, 4),
                    new(5, 4),
                    new(5, 5),
                    new(5, 6),
                    new(4, 6),
                    new(3, 6),
                    new(3, 5)
                )
            };
        }

        private static IEnumerable<object[]> ContainedNonDiagonalNeighbors()
        {
            Bounds bounds = new(new(3, 4), new(10, 5));

            // Neighbors for top left corner
            yield return new object[]
            {
                new Location(3, 4),
                ImmutableList.Create<Location>
                (
                    new(4, 4),
                    new(3, 5)
                )
            };

            // Neighbors for top right corner
            yield return new object[]
            {
                new Location(12, 4),
                ImmutableList.Create<Location>
                (
                    new(11, 4),
                    new(12, 5)
                )
            };

            // Neighbors for bottom right corner
            yield return new object[]
            {
                new Location(12, 8),
                ImmutableList.Create<Location>
                (
                    new(11, 8),
                    new(12, 7)
                )
            };

            // Neighbors for bottom left corner
            yield return new object[]
            {
                new Location(3, 8),
                ImmutableList.Create<Location>
                (
                    new(4, 8),
                    new(3, 7)
                )
            };

            // Neighbors for a location away from all edges
            yield return new object[]
            {
                new Location(4, 5),
                ImmutableList.Create<Location>
                (
                    new(4, 4),
                    new(5, 5),
                    new(4, 6),
                    new(3, 5)
                )
            };
        }
    }

    public class GettingLocationsTests
    {
        [Theory]
        [InlineData(1, 8)]
        [InlineData(2, 16)]
        [InlineData(3, 11)]
        internal void Getting_the_count_of_locations_at_a_distance_with_diagonal_nodes_allowed(int distance, int expectedLocationCount)
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            int locationCount = sut.GetLocationCount(new Location(5, 6), distance, allowDiagonal: true);

            locationCount.Should().Be(expectedLocationCount);
        }

        [Theory]
        [InlineData(1, 4)]
        [InlineData(2, 8)]
        [InlineData(3, 10)]
        internal void Getting_the_count_of_locations_at_a_distance_with_no_diagonal_nodes_allowed(int distance, int expectedLocationCount)
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            int locationCount = sut.GetLocationCount(new Location(5, 6), distance, allowDiagonal: false);

            locationCount.Should().Be(expectedLocationCount);
        }

        [Fact]
        internal void Getting_a_list_of_all_locations_within_a_bounds()
        {
            Bounds sut = new(new Location(3, 4), new Size(2, 3));

            ImmutableList<Location> locations = sut.GetLocations();

            locations.Count.Should().Be(6);
            locations.Should().Contain(new Location(3, 4));
            locations.Should().Contain(new Location(4, 4));
            locations.Should().Contain(new Location(3, 5));
            locations.Should().Contain(new Location(4, 5));
            locations.Should().Contain(new Location(3, 6));
            locations.Should().Contain(new Location(4, 6));
        }

        [Fact]
        internal void Finding_locations_at_a_distance_of_1_with_no_diagonal_movement_allowed()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 1, allowDiagonal: false);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(4);
            locations.Should().Contain(new Location(4, 6));
            locations.Should().Contain(new Location(6, 6));
            locations.Should().Contain(new Location(5, 5));
            locations.Should().Contain(new Location(5, 7));
        }

        [Fact]
        internal void Finding_locations_at_a_distance_of_2_with_no_diagonal_movement_allowed()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 2, allowDiagonal: false);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(8);
            locations.Should().Contain(new Location(5, 8));
            locations.Should().Contain(new Location(5, 4));
            locations.Should().Contain(new Location(7, 6));
            locations.Should().Contain(new Location(3, 6));
            locations.Should().Contain(new Location(6, 7));
            locations.Should().Contain(new Location(4, 7));
            locations.Should().Contain(new Location(6, 5));
            locations.Should().Contain(new Location(4, 5));
        }

        [Fact]
        internal void
            Finding_locations_in_a_range_from_1_to_2_with_no_diagonal_movement_allowed_merges_the_locations_at_those_distances()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 1, 2, allowDiagonal: false);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(12);
            locations.Should().Contain(new Location(4, 6));
            locations.Should().Contain(new Location(6, 6));
            locations.Should().Contain(new Location(5, 5));
            locations.Should().Contain(new Location(5, 7));
            locations.Should().Contain(new Location(5, 8));
            locations.Should().Contain(new Location(5, 4));
            locations.Should().Contain(new Location(7, 6));
            locations.Should().Contain(new Location(3, 6));
            locations.Should().Contain(new Location(6, 7));
            locations.Should().Contain(new Location(4, 7));
            locations.Should().Contain(new Location(6, 5));
            locations.Should().Contain(new Location(4, 5));
        }

        [Fact]
        internal void Finding_locations_at_a_distance_of_1_with_diagonal_movement_allowed()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 1, allowDiagonal: true);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(8);
            locations.Should().Contain(new Location(5, 7));
            locations.Should().Contain(new Location(5, 5));
            locations.Should().Contain(new Location(4, 6));
            locations.Should().Contain(new Location(6, 6));
            locations.Should().Contain(new Location(4, 5));
            locations.Should().Contain(new Location(4, 7));
            locations.Should().Contain(new Location(6, 5));
            locations.Should().Contain(new Location(6, 7));
        }

        [Fact]
        internal void Finding_locations_at_a_distance_of_2_with_diagonal_movement_allowed()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 2, allowDiagonal: true);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(16);
            // All orthogonal locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(5, 8));
            locations.Should().Contain(new Location(5, 4));
            locations.Should().Contain(new Location(3, 6));
            locations.Should().Contain(new Location(7, 6));
            // All diagonal locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(3, 4));
            locations.Should().Contain(new Location(7, 4));
            locations.Should().Contain(new Location(7, 8));
            locations.Should().Contain(new Location(3, 8));
            // All other locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(3, 5));
            locations.Should().Contain(new Location(3, 7));
            locations.Should().Contain(new Location(7, 5));
            locations.Should().Contain(new Location(7, 7));
            locations.Should().Contain(new Location(4, 4));
            locations.Should().Contain(new Location(4, 8));
            locations.Should().Contain(new Location(6, 4));
            locations.Should().Contain(new Location(6, 8));
        }

        [Fact]
        internal void
            Finding_locations_in_a_range_from_1_to_2_with_diagonal_movement_allowed_merges_the_locations_at_those_distances()
        {
            Bounds sut = new(new Location(3, 4), new Size(10, 10));

            IEnumerable<Location> locations = sut.GetLocations(new Location(5, 6), 1, 2, allowDiagonal: true);

            locations.Should().NotBeNull();
            locations.Should().HaveCount(24);
            locations.Should().Contain(new Location(5, 7));
            locations.Should().Contain(new Location(5, 5));
            locations.Should().Contain(new Location(4, 6));
            locations.Should().Contain(new Location(6, 6));
            locations.Should().Contain(new Location(4, 5));
            locations.Should().Contain(new Location(4, 7));
            locations.Should().Contain(new Location(6, 5));
            locations.Should().Contain(new Location(6, 7));
            // All orthogonal locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(5, 8));
            locations.Should().Contain(new Location(5, 4));
            locations.Should().Contain(new Location(3, 6));
            locations.Should().Contain(new Location(7, 6));
            // All diagonal locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(3, 4));
            locations.Should().Contain(new Location(7, 4));
            locations.Should().Contain(new Location(7, 8));
            locations.Should().Contain(new Location(3, 8));
            // All other locations 2 steps from (5, 6)
            locations.Should().Contain(new Location(3, 5));
            locations.Should().Contain(new Location(3, 7));
            locations.Should().Contain(new Location(7, 5));
            locations.Should().Contain(new Location(7, 7));
            locations.Should().Contain(new Location(4, 4));
            locations.Should().Contain(new Location(4, 8));
            locations.Should().Contain(new Location(6, 4));
            locations.Should().Contain(new Location(6, 8));
        }
    }
}
