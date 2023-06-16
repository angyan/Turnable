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

public class CharacterLocationsTests
{
    [Fact]
    internal void The_initial_list_of_locations_should_be_empty()
    {
        // No Arrange

        // Act
        CharacterLocations sut = new();

        // Assert
        sut.Value.Should().BeEmpty();
    }

    [Fact]
    internal void A_characters_location_can_be_set_given_a_map()
    {
        Map map = Map.Load(
            "../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        CharacterLocations sut = new();
        Character character = CreateCharacter("Test");
        Location location = new(1, 1);

        sut = sut.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }));

        sut.Value.Should().NotBeEmpty();
        sut.Value.Count.Should().Be(1);
        sut.Value[character].Should().Be(new Location(1, 1));
    }

    [Fact]
    internal void A_character_cannot_be_added_twice_at_the_same_location()
    {
        Map map = Map.Load(
            "../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        CharacterLocations sut = new();
        Character character = CreateCharacter("Test");
        Location location = new(1, 1);
        sut = sut.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }));

        // No Act
        
        sut.Invoking(s => s.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }))).Should().Throw<ArgumentException>().WithMessage($"Character '{character.Name}' cannot be added twice at {location}");
    }

    [Fact]
    internal void A_character_cannot_be_added_twice_at_different_locations()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        CharacterLocations sut = new();
        Character character = CreateCharacter("Test");
        Location location = new(1, 1);
        Location location2 = new(2, 1);
        sut = sut.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }));

        // No Act
        
        sut.Invoking(s => s.AddOnMap(character, location2, map, new CollisionMasks(new[] { 1 }))).Should().Throw<ArgumentException>().WithMessage($"Character '{character.Name}' cannot be added at {location} and then at {location2}");
    }

    [Fact]
    internal void A_character_cannot_be_added_at_the_location_of_an_obstacle()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        CharacterLocations sut = new();
        Character character = CreateCharacter("Test");
        Location location = new(0, 0);
        
        // No act

        sut.Invoking(s => s.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }))).Should().Throw<ArgumentException>().WithMessage($"Character '{character.Name}' cannot be added at {location} because there is an obstacle there");
    }

    [Fact]
    void A_character_cannot_be_added_at_the_location_of_another_character()
    {
        MapFilePath mapFilePath = new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
        MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
        Map map = mapJsonString.Deserialize();
        CharacterLocations sut = new();
        Character character = CreateCharacter("Test");
        Character character2 = CreateCharacter("Test2");
        Location location = new(1, 1);
        sut = sut.AddOnMap(character, location, map, new CollisionMasks(new[] { 1 }));

        // No act

        sut.Invoking(s => s.AddOnMap(character2, location, map, new CollisionMasks(new[] { 1 }))).Should().Throw<ArgumentException>().WithMessage($"Character '{character2.Name}' cannot be added at {location} because another character is already located at {location}");
    }

    private Character CreateCharacter(string name) => new(name, Abilities: ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);
}