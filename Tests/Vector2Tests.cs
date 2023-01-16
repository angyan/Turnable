using Turnable.AI.Pathfinding;
using FluentAssertions;
using System.Reflection;

namespace Tests;

public class Vector2Tests
{
    [Fact]
    public void Comparing_equal_vector2s()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(1, 2);

        bool areEqual = Vector2.Equals(vector1, vector2);

        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Comparing_different_vector2s()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(2, 3);

        bool areEqual = Vector2.Equals(vector1, vector2);

        areEqual.Should().BeFalse();
    }

    [Fact]
    public void Comparing_to_another_equal_vector2()
    {
        var sut = new Vector2(1, 2);
        var vector = new Vector2(1, 2);

        bool areEqual = sut.Equals(vector);

        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Comparing_to_a_different_vector2()
    {
        var sut = new Vector2(1, 2);
        var vector = new Vector2(2, 3);

        bool areEqual = sut.Equals(vector);

        areEqual.Should().BeFalse();
    }

    [Fact]
    public void Comparing_to_an_object_reference_to_an_equal_vector2()
    {
        var sut = new Vector2(1, 2);
        Object vector = new Vector2(1, 2);

        bool areEqual = sut.Equals(vector);

        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Comparing_to_an_object_reference_to_a_different_vector2()
    {
        var sut = new Vector2(1, 2);
        Object vector = new Vector2(2, 3);

        bool areEqual = sut.Equals(vector);

        areEqual.Should().BeFalse();
    }

    [Fact]
    public void Comparing_equal_vector2s_using_the_equality_operator()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(1, 2);

        bool areEqual = vector1 == vector2;

        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Comparing_different_vector2s_using_the_equality_operator()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(2, 3);

        bool areEqual = vector1 == vector2;

        areEqual.Should().BeFalse();
    }

    [Fact]
    public void Comparing_equal_vectors2s_using_the_inequality_operator()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(1, 2);

        bool areEqual = vector1 != vector2;

        areEqual.Should().BeFalse();
    }

    [Fact]
    public void Comparing_different_vector2s_using_the_inequality_operator()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(2, 3);

        bool areEqual = vector1 != vector2;

        areEqual.Should().BeTrue();
    }

    [Fact]
    public void Calculating_a_hash_code()
    {
        var sut = new Vector2(1, 2);

        int hashCode = sut.GetHashCode();

        hashCode.Should().Be(35);
    }

    [Fact]
    public void Getting_a_human_readable_representation()
    {
        var sut = new Vector2(1, 2);

        string s = sut.ToString();

        s.Should().Be("(1, 2)");
    }

    [Fact]
    public void Addition()
    {
        var vector1 = new Vector2(1, 2);
        var vector2 = new Vector2(2, 3);

        Vector2 vector3 = vector1 + vector2;

        vector3.Should().Be(new Vector2(3, 5));
    }

    [Theory]
    [InlineData(1, 2, -1, -2)]
    [InlineData(-1, -2, 1, 2)]
    public void Negation(int x, int y, int negatedX, int negatedY)
    {
        var vector = new Vector2(x, y);

        Vector2 negatedVector = -vector;

        negatedVector.Should().Be(new Vector2(negatedX, negatedY));
    }

    [Fact]
    public void Subtraction()
    {
        var vector1 = new Vector2(3, 5);
        var vector2 = new Vector2(1, 2);

        Vector2 vector3 = vector1 - vector2;

        vector3.Should().Be(new Vector2(2, 3));
    }

    [Fact]
    public void Calculating_the_magnitude()
    {
        var sut = new Vector2(3, 4);

        double magnitude = sut.Magnitude;

        magnitude.Should().Be(5.0);
    }

    [Fact]
    public void Calculating_distance_between_two_vector2s()
    {
        var vector1 = new Vector2(4, 6);
        var vector2 = new Vector2(1, 2);

        double distance = Vector2.Distance(vector1, vector2);

        distance.Should().Be(5.0);
    }

    [Fact]
    public void Calculating_distance_to_another_vector2()
    {
        var sut = new Vector2(4, 6);
        var vector = new Vector2(1, 2);

        double distance = sut.Distance(vector);

        distance.Should().Be(5.0);
    }
}
