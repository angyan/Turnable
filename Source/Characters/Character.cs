using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record Character(string Name, IImmutableDictionary<string, Ability> Abilities,
    IImmutableDictionary<string, Skill> Skills) : IComparable
{
    public int CompareTo(object? obj) => CompareTo((Character) obj!);

    public int CompareTo(Character other) => Name.CompareTo(other.Name);
}
