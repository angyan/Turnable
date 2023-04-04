using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record Character(string Name, IImmutableList<Ability> Abilities, Location Location, ImmutableList<Skill> Skills);
