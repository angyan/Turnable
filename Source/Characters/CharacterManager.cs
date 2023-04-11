using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.Characters;

internal static class CharacterManager
{
    internal static Stat Update(this Stat stat, int newValue)
    {
        if (newValue <= stat.Minimum)
        {
            stat.RaiseMinimumReached(newValue);
            return stat with { Value = stat.Minimum };
        }
        else if (newValue >= stat.Maximum)
        {
            stat.RaiseMaximumReached(newValue);
            return stat with { Value = stat.Maximum };
        }
        else if (newValue != stat.Value)
        {
            stat.RaiseValueUpdated(newValue);
            return stat with { Value = newValue };
        }
        else
        {
            return stat;
        }
    }

    internal static CharacterMoves Move(this CharacterMoves characterMoves, Character character, Location location) =>
        characterMoves.Value.ContainsKey(character)
            ? new CharacterMoves(
                Value: characterMoves.Value.SetItem(character, characterMoves.Value[character].Add(location)))
            : new CharacterMoves(Value: characterMoves.Value.Add(character, ImmutableList.Create(location)));

    internal static ImmutableDictionary<Character, Location> GetCurrentCharacterLocations(
        this CharacterMoves characterMoves) =>
        characterMoves.Value.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.Last());

    internal static ImmutableDictionary<Character, Location> GetCurrentCharacterLocations(
        this CharacterMoves characterMoves, Character characterToExclude) =>
        characterMoves.GetCurrentCharacterLocations().Remove(characterToExclude);

    internal static ImmutableList<Location>
        GetCharacterMoves(this CharacterMoves characterMoves, Character character) =>
        characterMoves.Value[character].Reverse();
}
