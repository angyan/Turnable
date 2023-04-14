using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record Character(string Name, IImmutableDictionary<string, Ability> Abilities, IImmutableDictionary<string, Skill> Skills);
