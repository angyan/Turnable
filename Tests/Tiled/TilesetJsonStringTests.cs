using FluentAssertions;
using Turnable.Tiled;

namespace Tests.Tiled;

public class TilesetJsonStringTests
{
    [Fact]
    internal void Json_cannot_be_constructed_with_the_string_null()
    {
        // No arrange

        Action construction = () => new TilesetJsonString("null");

        construction.Should().Throw<ArgumentException>().WithMessage(@"""null"" is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Json_cannot_be_constructed_with_a_value_of_null()
    {
        // No arrange

        Action construction = () => new TilesetJsonString(null);

        construction.Should().Throw<ArgumentException>().WithMessage(@"null is not a valid value for constructing a JsonString");
    }

    [Fact]
    internal void Tileset_json_string_can_be_implicitly_converted_to_a_string()
    {
        TilesetJsonString tilesetJsonString = new("{}");

        string tilesetJson = tilesetJsonString ;

        tilesetJson.Should().Be("{}");
    }

    [Fact]
    internal void String_can_be_implicitly_converted_to_a_tileset_json_string()
    {
        string tilesetJson = "{}";

        TilesetJsonString tilesetJsonString = tilesetJson;

        tilesetJsonString.Value.Should().Be("{}");
    }
}