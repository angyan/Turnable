using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json.Linq;
using Turnable.AI.Pathfinding;
using Turnable.Characters;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.Skills;
using Turnable.Tiled;
using Turnable.TiledMap;
using Ability = Turnable.Characters.Ability;

namespace Tests.Characters;

public class CharacterTurnsTests
{
    [Fact]
    internal void An_initial_sorted_set_of_characters_can_be_used_to_initialize_the_character_turns()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        Character character3 = CreateCharacter("Test3");
        ImmutableSortedSet<Character>.Builder builder = ImmutableSortedSet.CreateBuilder<Character>();
        builder.Add(character1);
        builder.Add(character2);
        builder.Add(character3);
        ImmutableSortedSet<Character> initialCharactersTurns = builder.ToImmutable();

        CharacterTurns sut = new(initialCharactersTurns);

        sut.Value.Should().NotBeNull();
        sut.Value.Count.Should().Be(3);
        sut.Value.Should().BeEquivalentTo(initialCharactersTurns);
        sut.Current.Should().Be(character1);
    }

    [Fact]
    internal void The_character_turns_can_be_advanced()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        Character character3 = CreateCharacter("Test3");
        ImmutableSortedSet<Character>.Builder builder = ImmutableSortedSet.CreateBuilder<Character>();
        builder.Add(character1);
        builder.Add(character2);
        builder.Add(character3);
        ImmutableSortedSet<Character> initialCharactersTurns = builder.ToImmutable();
        CharacterTurns sut = new(initialCharactersTurns);

        CharacterTurns newCharacterTurns = sut.Advance();

        newCharacterTurns.Value.Should().NotBeNull();
        newCharacterTurns.Value.Count.Should().Be(3);
        newCharacterTurns.Value.Should().BeEquivalentTo(initialCharactersTurns);
        newCharacterTurns.Current.Should().Be(character2);
    }

    [Fact]
    internal void The_character_turns_rotate_back_to_the_first_character()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        Character character3 = CreateCharacter("Test3");
        ImmutableSortedSet<Character>.Builder builder = ImmutableSortedSet.CreateBuilder<Character>();
        builder.Add(character1);
        builder.Add(character2);
        builder.Add(character3);
        ImmutableSortedSet<Character> initialCharactersTurns = builder.ToImmutable();
        CharacterTurns sut = new(initialCharactersTurns);

        CharacterTurns newCharacterTurns = sut.Advance().Advance().Advance();

        newCharacterTurns.Value.Should().NotBeNull();
        newCharacterTurns.Value.Count.Should().Be(3);
        newCharacterTurns.Value.Should().BeEquivalentTo(initialCharactersTurns);
        newCharacterTurns.Current.Should().Be(character1);
    }

    private Character CreateCharacter(string name) => new(name, Abilities: ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);
}