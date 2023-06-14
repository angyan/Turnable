using System.Collections.Immutable;
using Turnable.Characters;
using Turnable.TiledMap;

namespace Turnable.Game;

internal record State(Map Map, IImmutableList<Character> Characters, CharacterMoves CharacterMoves,
    CharacterTurns CharacterTurns, ImmutableList<Func<State, State>> PendingUpdates);
