using FluentAssertions;
using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;
using Path = Turnable.AI.Pathfinding.Path;

namespace Tests.AI.Pathfinding;

public class PathTests
{
    [Fact]
    internal void Two_paths_with_the_same_nodes_are_equal()
    {
        ImmutableList<Location> nodes1 = ImmutableList.Create<Location>(new(1, 1), new(1, 2), new(1, 3));
        ImmutableList<Location> nodes2 = ImmutableList.Create<Location>(new(1, 1), new(1, 2), new(1, 3));

        Path sut = new(nodes1);
        Path path2 = new(nodes2);

        sut.Should().Be(path2);
    }

    [Theory]
    [MemberData(nameof(DifferentPathNodes))]
    internal void Two_paths_with_nodes_in_a_different_sequence_or_different_nodes_are_not_equal(ImmutableList<Location> nodes1, ImmutableList<Location> nodes2)
    {
        // No arrange

        Path sut = new(nodes1);
        Path path2 = new(nodes2);

        sut.Should().NotBe(path2);
    }

    [Fact]
    internal void Returning_the_count_of_all_nodes_in_a_path()
    {
        ImmutableList<Location> nodes = ImmutableList.Create<Location>(new(1, 1), new(1, 2), new(1, 3));
        Path sut = new(nodes);

        int count = sut.Count;

        count.Should().Be(3);
    }

    [Fact]
    internal void Indexing_into_the_nodes_in_the_path()
    {
        ImmutableList<Location> nodes = ImmutableList.Create<Location>(new(1, 1), new(1, 2), new(1, 3));
        Path sut = new(nodes);

        Location node = sut[0];

        node.Should().NotBeNull();
        node.Should().Be(new Location(1, 1));
    }

    private static IEnumerable<object[]> DifferentPathNodes()
    {
        // Nodes in reverse order of path being compared to
        yield return new object[]
        {
            ImmutableList.Create<Location>
            (
                new(2, 4),
                new(2, 5),
                new(2, 6)
            ),
            ImmutableList.Create<Location>
            (                
                new(2, 6),
                new(2, 5),
                new(2, 4)
            )
        };

        // Nodes in different order of path being compared to
        yield return new object[]
        {
            ImmutableList.Create<Location>
            (
                new(2, 4),
                new(2, 5),
                new(2, 6)
            ),
            ImmutableList.Create<Location>
            (                
                new(2, 4),
                new(2, 6),
                new(2, 5)
            )
        };


        // Completely different nodes
        yield return new object[]
        {
            ImmutableList.Create<Location>
            (
                new(2, 4),
                new(2, 5),
                new(2, 6)
            ),
            ImmutableList.Create<Location>
            (                
                new(7, 8),
                new(8, 6),
                new(11, 5)
            )
        };


    }
}
