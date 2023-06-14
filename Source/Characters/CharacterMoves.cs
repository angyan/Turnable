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
        if (map.GetObstacles(collisionMasks).Contains(location))
        {
            throw new ArgumentException();
        }

        if (GetLocations().Values.Contains(location))
        {
            throw new ArgumentException();
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
