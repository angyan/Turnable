using System.Collections.Immutable;
using FluentAssertions;
using Turnable.Characters;
using Turnable.Skills;
using Ability = Turnable.Characters.Ability;

namespace Tests.Skills;

public class EffectTests
{
    [Fact]
    internal void A_stat_decrease_effect_decreases_a_stat_by_an_amount()
    {
        Stat targetStat = new(50, 10, 100);
        Func<Stat, int, Stat> sut = Effects.StatDecreaseFunc;

        Stat newStat = sut(targetStat, 10);

        newStat.Value.Should().Be(40);
    }

    [Fact]
    internal void A_stat_increase_effect_increases_a_stat_by_an_amount()
    {
        Stat targetStat = new(50, 10, 100);
        Func<Stat, int, Stat> sut = Effects.StatIncreaseFunc;

        Stat newStat = sut(targetStat, 10);

        newStat.Value.Should().Be(60);
    }

    [Fact]
    internal void A_character_stat_decrease_effect_decreases_the_stat_for_an_ability_for_a_character_by_an_amount()
    {
        Stat targetStat = new(50, 10, 100);
        Ability ability = new("Hit Points", targetStat);
        Character character = new("Test", ImmutableDictionary<string, Ability>.Empty.Add("HP", ability), ImmutableDictionary<string, Skill>.Empty);
        Func<Character, string, int, Character> sut = Effects.CharacterStatDecreaseFunc;

        Character newCharacter = sut(character, "HP", 10);

        Ability hpAbility = newCharacter.Abilities["HP"];
        hpAbility.Stat.Value.Should().Be(40);
    }

    [Fact]
    internal void A_character_stat_increase_effect_increases_the_stat_for_an_ability_for_a_character_by_an_amount()
    {
        Stat targetStat = new(50, 10, 100);
        Ability ability = new("Hit Points", targetStat);
        Character character = new("Test", ImmutableDictionary<string, Ability>.Empty.Add("HP", ability), ImmutableDictionary<string, Skill>.Empty);
        Func<Character, string, int, Character> sut = Effects.CharacterStatIncreaseFunc;

        Character newCharacter = sut(character, "HP", 10);

        Ability hpAbility = newCharacter.Abilities["HP"];
        hpAbility.Stat.Value.Should().Be(60);
    }

}