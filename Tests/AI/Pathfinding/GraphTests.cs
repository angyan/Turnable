using FluentAssertions;
using System.Collections.Immutable;
using Turnable.AI.Pathfinding;
using Turnable.Layouts;

namespace Tests.AI.Pathfinding;

public class GraphTests
{
    [Fact]
    internal void A_graph_can_be_implicitly_cast_to_an_immutable_dictionary()
    {
        Graph sut = new(ImmutableDictionary<Location, ImmutableList<Location>>.Empty);

        ImmutableDictionary<Location, ImmutableList<Location>> result = sut;

        result.Should().NotBeNull();
    }

    [Fact]
    internal void An_immutable_dictionary_can_be_implicitly_cast_to_a_graph()
    {
        ImmutableDictionary<Location, ImmutableList<Location>> sut = ImmutableDictionary<Location, ImmutableList<Location>>.Empty;

        Graph result = sut;

        result.Value.Should().BeEquivalentTo(ImmutableDictionary<Location, ImmutableList<Location>>.Empty);
    }
}
