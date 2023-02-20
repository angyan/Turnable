using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

internal record Graph(ImmutableDictionary<Location, ImmutableList<Location>> Value)
{
    public static implicit operator ImmutableDictionary<Location, ImmutableList<Location>>(Graph graph) => graph.Value; 
    public static implicit operator Graph(ImmutableDictionary<Location, ImmutableList<Location>> value) => new(value);
}
