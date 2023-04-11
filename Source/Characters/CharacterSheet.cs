using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal static class CharacterSheet
{
    internal static Ability FindAbility(this Character character, string name) => character.Abilities.FirstOrDefault(a => a.Name == name);
}