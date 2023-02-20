using FluentAssertions;
using Turnable.Layouts;

namespace Tests.Layouts;

public class DimensionTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    internal void A_dimension_is_positive_and_greater_than_zero(int value)
    {
        // No arrange

        Action construction = () => new Dimension(value);

        construction.Should().Throw<ArgumentException>()
            .WithMessage($"{value} is not a valid value for a Dimension; it has to be positive and greater than 0");
    }

    [Fact]
    internal void A_dimension_can_be_implicitly_cast_to_an_int()
    {
        Dimension sut = new Dimension(1);

        int result = sut;

        result.Should().Be(1);
    }

    [Fact]
    internal void An_int_can_be_implicitly_cast_to_a_dimension()
    {
        int sut = 1;

        Dimension result = sut;

        result.Value.Should().Be(1);
    }
}
