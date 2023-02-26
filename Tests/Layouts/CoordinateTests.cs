using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts;

public class CoordinateTests
{
    [Fact]
    internal void A_coordinate_can_be_implicitly_cast_to_an_int()
    {
        Coordinate sut = new(1);

        int result = sut;

        result.Should().Be(1);
    }

    [Fact]
    internal void An_int_can_be_implicitly_cast_to_a_coordinate()
    {
        int sut = 1;

        Coordinate result = sut;

        result.Value.Should().Be(1);
    }
}
