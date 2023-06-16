using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.TiledMap;

namespace Turnable.Characters;

internal record CharacterMoves(ImmutableDictionary<Character, ImmutableList<Location>> Value)
{
    internal CharacterMoves(CharacterLocations characterLocations) : this(ImmutableDictionary<Character, ImmutableList<Location>>.Empty)
    {
        Value = characterLocations.Value.ToImmutableDictionary(kvp => kvp.Key, kvp => ImmutableList.Create(kvp.Value));
    }

    internal CharacterMoves Move(Character character, Location location, Map map, CollisionMasks collisionMasks)
    {
        // Is the character being moved to a location that is an obstacle?
        if (map.GetObstacles(collisionMasks).Contains(location))
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be moved to {location} because there is an obstacle there");
        }

        // Is the character being moved to the same location?
        if (Value.ContainsKey(character) && Value[character].Last() == location)
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be moved to {location} because it is already there");
        }

        // Is the character being moved to a location that is already occupied by another character?
        if (GetLocations().Values.Contains(location))
        {
            throw new ArgumentException($"Character '{character.Name}' cannot be added at {location} because another character is already at {location}");
        }

        return Value.ContainsKey(character)
            ? new CharacterMoves(Value.SetItem(character, Value[character].Add(location)))
            : new CharacterMoves(Value.Add(character, ImmutableList.Create(location)));
    }

    internal ImmutableDictionary<Character, Location> GetLocations(
    ) => Value.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.Last());

    internal ImmutableDictionary<Character, Location> GetLocations(
        Character characterToExclude) =>
        GetLocations().Remove(characterToExclude);

    internal ImmutableList<Location> GetMoves(Character character) =>
        Value[character].Reverse();

    internal int GetMovesCount(Character character) =>
        Value[character].Count - 1;
};
