using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

public record Path(IImmutableList<Location> Value)
{
    public virtual bool Equals(Path? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.SequenceEqual(other.Value);
    }

    internal int NodeCount => Value.Count;

    internal Location this[int index] => Value[index];
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
}
