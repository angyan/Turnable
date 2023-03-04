using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

public record Graph(ImmutableDictionary<Location, ImmutableList<Location>> Value)
{
    public int Count => Value.Count;

    public ImmutableList<Location> this[Location location] => Value[location];
}
