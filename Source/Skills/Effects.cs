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
            Ability ability = character.Abilities[abilityName];
            Ability newAbility = ability with
            {
                Stat = StatDecreaseFunc(ability.Stat, amount)
            };

            return character with
            {
                Abilities = character.Abilities.SetItem(abilityName, newAbility)
            };
        };

    internal static Func<Character, string, int, Character> CharacterStatIncreaseFunc =
        (Character character, string abilityName, int amount) =>
        {
            Ability ability = character.Abilities[abilityName];
            Ability newAbility = ability with
            {
                Stat = StatIncreaseFunc(ability.Stat, amount)
            };

            return character with
            {
                Abilities = character.Abilities.SetItem(abilityName, newAbility)
            };
        };
}