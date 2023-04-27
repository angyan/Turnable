using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Skills;

namespace Turnable.Characters;

internal record CharacterMoves(ImmutableDictionary<Character, ImmutableList<Location>> Value)
{
    internal CharacterMoves Move(Character character, Location location) =>
        Value.ContainsKey(character)
            ? new CharacterMoves(Value.SetItem(character, Value[character].Add(location)))
            : new CharacterMoves(Value.Add(character, ImmutableList.Create(location)));

    internal ImmutableDictionary<Character, Location> GetLocations(
    ) => Value.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.Last());

    internal ImmutableDictionary<Character, Location> GetLocations(
        Character characterToExclude) =>
        GetLocations().Remove(characterToExclude);

    internal ImmutableList<Location> GetMoves(Character character) =>
        Value[character].Reverse();
};
