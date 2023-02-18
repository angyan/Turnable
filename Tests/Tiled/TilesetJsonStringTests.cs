using FluentAssertions;
using Turnable.Tiled;
using Turnable.TiledMap;
using Xunit.Abstractions;

namespace Tests.Tiled
{
    public class TilesetJsonStringTests
    {
        [Fact]
        public void Json_cannot_be_constructed_with_the_string_null()
        {
            // No arrange

            Action construction = () => new TilesetJsonString("null");

            construction.Should().Throw<ArgumentException>().WithMessage(@"""null"" is not a valid value for constructing a JsonString");
        }

        [Fact]
        public void Json_cannot_be_constructed_with_a_value_of_null()
        {
            // No arrange

            Action construction = () => new TilesetJsonString(null);

            construction.Should().Throw<ArgumentException>().WithMessage(@"null is not a valid value for constructing a JsonString");
        }
    }
}

