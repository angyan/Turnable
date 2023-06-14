namespace Tests.AI.Plans;

public class PlannerTests
{
    [Fact]
    internal void An_action_to_move_next_to_another_character_can_be_planned()
    {
    }

    //// Factory method to create state for a game
    //private static State CreateGameState()
    //{
    //    MapFilePath mapFilePath =
    //        new("../../../Fixtures/orthogonal_csv_right_down_map_dimensions_16x16_tile_dimensions_32x32_not_empty.tmj");
    //    MapJsonString mapJsonString = new(File.ReadAllText(mapFilePath));
    //    Map map = mapJsonString.Deserialize();

    //    // Create two characters
    //    Character character = new("PC", ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);
    //    Character character2 = new("NPC", ImmutableDictionary<string, Ability>.Empty, ImmutableDictionary<string, Skill>.Empty);

    //    // Add the characters to the map
    //    ImmutableDictionary<Character, ImmutableList<Location>> initialCharacterLocations = ImmutableDictionary<Character, ImmutableList<Location>>.Empty;
        

    //    return new State(map, ImmutableList<Character>.Empty.Add(character), new CharacterMoves(), new CharacterTurns(), ImmutableList<Func<State, State>>.Empty);
    //}
}