using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using FluentAssertions;
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

public class CharacterTests
{
    [Fact]
    internal void A_character_can_find_an_ability_by_name()
    {
        Stat stat = new(50, 10, 100);
        Ability abillity = new("HP", "Hit Points", stat);
        Character sut = new("Test", ImmutableList<Ability>.Empty.Add(abillity), new Location(0, 0), ImmutableList<Skill>.Empty);

        Ability foundAbility = sut.FindAbility("HP");

        foundAbility.Should().NotBeNull();
        foundAbility.Name.Should().Be("HP");
        foundAbility.Description.Should().Be("Hit Points");
        foundAbility.Stat.Value.Should().Be(50);
    }
}