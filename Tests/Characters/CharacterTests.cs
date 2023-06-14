using System.Collections.Immutable;
using FluentAssertions;
using Turnable.Characters;
using Turnable.Skills;
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