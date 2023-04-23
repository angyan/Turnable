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
    internal void Characters_implement_the_comparable_interface()
    {
        Character sut = new Character("Test", ImmutableDictionary<string, Ability>.Empty,
            ImmutableDictionary<string, Skill>.Empty);

        // No Act

        // Assert
        sut.GetType().Should().Implement<IComparable>();
    }
}