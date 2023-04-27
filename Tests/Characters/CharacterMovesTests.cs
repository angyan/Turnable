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

public class CharacterMovesTests
{
    [Fact]
    internal void An_initial_dictionary_of_character_locations_can_be_used_to_initialize_the_character_moves()
    {
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        initialCharacterLocations  = initialCharacterLocations.Add(CreateCharacter("Test"), ImmutableList<Location>.Empty.Add(new Location(0, 0)));

        CharacterMoves sut = new(initialCharacterLocations);

        sut.Value.Should().NotBeNull();
        sut.Value.Count.Should().Be(1);
        sut.Value.Should().BeEquivalentTo(initialCharacterLocations);
    }

    [Fact]
    internal void A_character_that_already_has_a_location_can_be_moved_to_a_new_location()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations  = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        initialCharacterLocations  = initialCharacterLocations.Add(character1, ImmutableList<Location>.Empty.Add(new Location(0, 0)));
        initialCharacterLocations  = initialCharacterLocations.Add(character2, ImmutableList<Location>.Empty.Add(new Location(0, 1)));
        CharacterMoves sut = new(initialCharacterLocations );

        CharacterMoves newCharacterMoves = sut.Move(character1, new Location(1, 1));

        ImmutableList<Location> character1Moves = newCharacterMoves.Value[character1];
        character1Moves.Count.Should().Be(2);
        character1Moves.First().Should().Be(new Location(0, 0));
        character1Moves.Last().Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void A_character_that_doesnt_have_a_location_is_added_with_an_initial_location_when_moved()
    {
        Character character1 = CreateCharacter("Test");
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations  = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        CharacterMoves sut = new(initialCharacterLocations );

        CharacterMoves newCharacterMoves = sut.Move(character1, new Location(1, 1));

        ImmutableList<Location> character1Moves = newCharacterMoves.Value[character1];
        character1Moves.Count.Should().Be(1);
        character1Moves.First().Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void The_current_location_of_each_character_is_the_last_location_that_character_moved_to()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations  = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        initialCharacterLocations  = initialCharacterLocations.Add(character1, ImmutableList<Location>.Empty.Add(new Location(0, 0)));
        initialCharacterLocations  = initialCharacterLocations.Add(character2, ImmutableList<Location>.Empty.Add(new Location(0, 1)));
        CharacterMoves sut = new(initialCharacterLocations );

        CharacterMoves newCharacterMoves = sut.Move(character1, new Location(1, 1));
        newCharacterMoves = newCharacterMoves.Move(character2, new Location(1, 2));
        newCharacterMoves = newCharacterMoves.Move(character1, new Location(2, 2));
        ImmutableDictionary<Character, Location> currentLocations = newCharacterMoves.GetLocations();

        currentLocations.Count.Should().Be(2);
        currentLocations[character1].Should().Be(new Location(2, 2));
        currentLocations[character2].Should().Be(new Location(1, 2));
    }

    [Fact]
    internal void The_current_locations_can_exclude_a_selected_character()
    {
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations  = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        initialCharacterLocations  = initialCharacterLocations.Add(character1, ImmutableList<Location>.Empty.Add(new Location(0, 0)));
        initialCharacterLocations  = initialCharacterLocations.Add(character2, ImmutableList<Location>.Empty.Add(new Location(0, 1)));
        CharacterMoves sut = new(initialCharacterLocations );

        CharacterMoves newCharacterMoves = sut.Move(character1, new Location(1, 1));
        newCharacterMoves = newCharacterMoves.Move(character2, new Location(1, 2));
        newCharacterMoves = newCharacterMoves.Move(character1, new Location(2, 2));
        ImmutableDictionary<Character, Location> currentLocations = newCharacterMoves.GetLocations(character2);

        currentLocations.Count.Should().Be(1);
        currentLocations[character1].Should().Be(new Location(2, 2));
    }

    [Fact]
    internal void The_moves_for_a_character_are_in_reverse_order()
    {
        Character character1 = CreateCharacter("Test");
        ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        initialCharacterLocations  = initialCharacterLocations.Add(character1, ImmutableList<Location>.Empty.Add(new Location(0, 0)));
        CharacterMoves sut = new(initialCharacterLocations );

        CharacterMoves newCharacterMoves = sut.Move(character1, new Location(1, 1));
        newCharacterMoves = newCharacterMoves.Move(character1, new Location(2, 2));
        ImmutableList<Location> character1Moves = newCharacterMoves.GetMoves(character1);

        character1Moves.Count.Should().Be(3);
        character1Moves.First().Should().Be(new Location(2, 2));
        character1Moves[1].Should().Be(new Location(1, 1));
        character1Moves.Last().Should().Be(new Location(0, 0));
    }

    private Character CreateCharacter(string name) => new(name, Abilities: ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);
}