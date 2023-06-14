using System.Collections.Immutable;
using FluentAssertions;
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
    internal void The_character_moves_can_be_initialized_with_an_initial_character_locations_instance()
    {
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(CreateCharacter("Test"), new Location(1, 1), CreateMap(),
                new CollisionMasks(new[] { 1 }));

        CharacterMoves sut = new(initialCharacterLocations);

        sut.Value.Should().NotBeNull();
        sut.Value.Count.Should().Be(1);
        sut.Value.First().Key.Name.Should().Be("Test");
        sut.Value.First().Value.Count.Should().Be(1);
        sut.Value.First().Value.First().Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void A_character_can_be_moved_to_a_new_location()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character2, new Location(3, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations);

        sut = sut.Move(character1, new Location(2, 1), map,
            new CollisionMasks(new[] { 1 }));

        ImmutableList<Location> character1Moves = sut.Value[character1];
        character1Moves.Count.Should().Be(2);
        character1Moves.First().Should().Be(new Location(1, 1));
        character1Moves.Last().Should().Be(new Location(2, 1));    }

    [Fact]
    internal void A_character_that_is_not_located_is_added_with_an_initial_location_when_moved()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        CharacterLocations initialCharacterLocations = new();
        CharacterMoves sut = new(initialCharacterLocations);

        sut = sut.Move(character1, new Location(1, 1), map,
            new CollisionMasks(new[] { 1 }));

        ImmutableList<Location> character1Moves = sut.Value[character1];
        character1Moves.Count.Should().Be(1);
        character1Moves.First().Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void A_character_cannot_be_moved_to_its_own_location()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        // No Act

        sut.Invoking(s => s.Move(character1, new Location(1, 1), map, new CollisionMasks(new[] { 1 })))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    internal void A_character_cannot_be_moved_to_another_characters_location()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                               new CollisionMasks(new[] { 1 }));
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character2, new Location(3, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        // No Act

        sut.Invoking(s => s.Move(character1, new Location(3, 1), map, new CollisionMasks(new[] { 1 })))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    internal void A_character_cannot_be_moved_to_an_obstacles_location()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        // No Act

        sut.Invoking(s => s.Move(character1, new Location(0, 0), map, new CollisionMasks(new[] { 1 })))
            .Should().Throw<ArgumentException>();
    }

    [Fact]
    internal void The_current_location_of_each_character_is_the_last_location_that__the_character_moved_to()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                               new CollisionMasks(new[] { 1 }));
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character2, new Location(3, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        sut = sut.Move(character1, new Location(2, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character2, new Location(4, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character1, new Location(2, 3), map, new CollisionMasks(new[] { 1 }));
        ImmutableDictionary<Character, Location> currentLocations = sut.GetLocations();

        currentLocations.Count.Should().Be(2);
        currentLocations[character1].Should().Be(new Location(2, 3));
        currentLocations[character2].Should().Be(new Location(4, 1));
    }

    [Fact]
    internal void The_current_locations_can_exclude_a_selected_characters_location()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character2, new Location(3, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        sut = sut.Move(character1, new Location(2, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character2, new Location(4, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character1, new Location(2, 3), map, new CollisionMasks(new[] { 1 }));
        ImmutableDictionary<Character, Location> currentLocations = sut.GetLocations(character2);

        currentLocations.Count.Should().Be(1);
        currentLocations[character1].Should().Be(new Location(2, 3));
    }

    [Fact]
    internal void The_moves_for_a_character_are_in_reverse_order()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        sut = sut.Move(character1, new Location(2, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character1, new Location(2, 3), map, new CollisionMasks(new[] { 1 }));
        ImmutableList<Location> character1Moves = sut.GetMoves(character1);

        character1Moves.Count.Should().Be(3);
        character1Moves.First().Should().Be(new Location(2, 3));
        character1Moves[1].Should().Be(new Location(2, 1));
        character1Moves.Last().Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void The_count_of_moves_for_a_character_is_one_less_than_the_number_of_locations_for_that_character()
    {
        Map map = CreateMap();
        Character character1 = CreateCharacter("Test");
        CharacterLocations initialCharacterLocations = new();
        initialCharacterLocations =
            initialCharacterLocations.AddOnMap(character1, new Location(1, 1), map,
                new CollisionMasks(new[] { 1 }));
        CharacterMoves sut = new(initialCharacterLocations );

        sut = sut.Move(character1, new Location(2, 1), map, new CollisionMasks(new[] { 1 }));
        sut = sut.Move(character1, new Location(2, 3), map, new CollisionMasks(new[] { 1 }));

        sut.GetMovesCount(character1).Should().Be(2);
    }

    private Map CreateMap()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();

        return map;
    }

    private Character CreateCharacter(string name) => new(name, Abilities: ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);
}