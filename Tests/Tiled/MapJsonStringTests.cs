using FluentAssertions;
using Turnable.Tiled;

namespace Tests.Tiled;

public class MapJsonStringTests
{
    [Fact]
    internal void Map_json_string_cannot_be_constructed_with_the_string_null()
    {
        // No arrange

        Action construction = () => new MapJsonString("null");

        construction.Should().Throw<ArgumentException>().WithMessage(@"""null"" is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Map_json_string_cannot_be_constructed_with_a_value_of_null()
    {
        // No arrange

        Action construction = () => new MapJsonString(null);

        construction.Should().Throw<ArgumentException>().WithMessage(@"null is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Map_json_string_can_be_implicitly_converted_to_a_string()
    {
        MapJsonString mapJsonString = new("{}");

        string mapJson = mapJsonString;

        mapJson.Should().Be("{}");
    }
}