using FluentAssertions;
using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;

namespace Tests.AI.Pathfinding;

public class PlanTests
{
    [Fact]
    internal void Indexing_into_the_dictionary_value()
    {
        Dictionary<Location, ImmutableList<Location>> dictionary = new()
        {
            { new(1, 1), ImmutableList.Create(new Location(1, 2)) },
            { new(1, 2), ImmutableList.Create(new Location(1, 1)) }
        };

        Graph sut = new(dictionary.ToImmutableDictionary());

        sut[new(1, 1)].Should().BeEquivalentTo(ImmutableList.Create(new Location(1, 2)));
    }

    [Fact]
    internal void Returning_the_count_of_nodes()
    {
        Dictionary<Location, ImmutableList<Location>> dictionary = new()
        {
            { new(1, 1), ImmutableList.Create(new Location(1, 2)) },
            { new(1, 2), ImmutableList.Create(new Location(1, 1)) }
        };

        Graph sut = new(dictionary.ToImmutableDictionary());

        sut.Count.Should().Be(2);
    }
}
