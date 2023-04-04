using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnable.Characters;
using Ability = Turnable.Characters.Ability;

namespace Turnable.Skills;

internal static class Effects
{
    internal static Func<Stat, int, Stat> StatDecreaseFunc = (Stat stat, int amount) => stat.Update(stat.Value - amount);
    internal static Func<Stat, int, Stat> StatIncreaseFunc = (Stat stat, int amount) => stat.Update(stat.Value + amount);

    internal static Func<Character, string, int, Character> CharacterStatDecreaseFunc =
        (Character character, string abilityName, int amount) =>
        {
            Ability ability = character.FindAbility(abilityName);
            Ability newAbility = ability with
            {
                Stat = StatDecreaseFunc(ability.Stat, amount)
            };

            return character with
            {
                Abilities = character.Abilities.Replace(ability, newAbility, new AbilityComparer())
            };
        };

    internal static Func<Character, string, int, Character> CharacterStatIncreaseFunc =
        (Character character, string abilityName, int amount) =>
        {
            Ability ability = character.FindAbility(abilityName);
            Ability newAbility = ability with
            {
                Stat = StatIncreaseFunc(ability.Stat, amount)
            };

            return character with
            {
                Abilities = character.Abilities.Replace(ability, newAbility, new AbilityComparer())
            };
        };

    internal class AbilityComparer : IEqualityComparer<Ability>
    {
        public bool Equals(Ability x, Ability y) => x.Name == y.Name;
        public int GetHashCode(Ability obj) => obj.Name.GetHashCode();
    }
}