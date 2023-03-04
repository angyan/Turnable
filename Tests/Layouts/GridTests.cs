using System.Collections.Immutable;
using FluentAssertions;
using Turnable.Layouts;
using Xunit.Abstractions;

namespace Tests.Layouts;

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

    [Theory]
    [MemberData(nameof(AllNeighbors))]
    internal void Getting_all_possible_neighbors(Location location, ImmutableList<Location> expectedNeighbors)
    {
        Bounds sut = new(new Location(3, 4), new Size(10, 5));

        ImmutableList<Location> neighbors = sut.GetNeighbors(location);

        neighbors.Should().BeEquivalentTo(expectedNeighbors);
    }

    [Theory]
    [MemberData(nameof(ContainedNeighbors))]
    internal void Getting_only_neighbors_contained_within_some_bounds(Location location,
        ImmutableList<Location> expectedNeighbors)
    {
        Bounds sut = new(new Location(3, 4), new Size(10, 5));

        ImmutableList<Location> neighbors = sut.GetContainedNeighbors(location);

        neighbors.Should().BeEquivalentTo(expectedNeighbors);
    }

    [Theory]
    [MemberData(nameof(ContainedNonDiagonalNeighbors))]
    internal void Getting_only_non_diagonal_neighbors_contained_within_some_bounds(Location location,
        ImmutableList<Location> expectedNeighbors)
    {
        Bounds sut = new(new Location(3, 4), new Size(10, 5));

        ImmutableList<Location> neighbors = sut.GetContainedNonDiagonalNeighbors(location);

        neighbors.Should().BeEquivalentTo(expectedNeighbors);
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

    private static IEnumerable<object[]> AllNeighbors()
    {
        // Neighbors for top left corner
        yield return new object[]
        {
            new Location(3, 4),
            ImmutableList.Create<Location>
            (
                new(2, 4),
                new(2, 3),
                new(3, 3),
                new(4, 3),
                new(4, 4),
                new(4, 5),
                new(3, 5),
                new(2, 5)
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

