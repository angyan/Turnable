using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

public record Path(ImmutableList<Location> Value)
{
    public virtual bool Equals(Path? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.SequenceEqual(other.Value);
    }

    internal int Count => Value.Count;

    internal Location this[int index] => Value[index];
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
