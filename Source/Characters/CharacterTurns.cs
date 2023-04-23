using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record CharacterTurns(ImmutableSortedSet<Character> Value, int Index = 0)
{
    internal Character Current => Value[Index];

    internal CharacterTurns Advance() => this with { Index = (Index + 1) % Value.Count};
}
