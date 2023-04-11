using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record CharacterMoves(ImmutableDictionary<Character, ImmutableList<Location>> Value);
