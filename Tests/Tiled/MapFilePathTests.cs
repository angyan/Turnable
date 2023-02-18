using FluentAssertions;
using Turnable.Tiled;
using Turnable.TiledMap;
using Xunit.Abstractions;

namespace Tests.Tiled
{
    public class MapFilePathTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(".tmx")]
        [InlineData(".TMX")]
        [InlineData(".txt")]
        public void Map_file_path_cannot_be_constructed_without_specifying_the_correct_extension(string extension)
        {
            // No arrange

            Action construction = () => new MapFilePath($"map{extension}");

            construction.Should().Throw<ArgumentException>().WithMessage($"{extension} is not a supported file extension for a Tiled Map");
        }

        [Theory]
        [InlineData(".tmj")]
        [InlineData(".TMJ")]
        [InlineData(".json")]
        [InlineData(".JSON")]
        public void Map_file_path_can_only_be_constructed_with_supported_file_extensions(string extension)
        {
            // No arrange

            Action construction = () => new MapFilePath($"map{extension}");

            construction.Should().NotThrow<ArgumentException>();
        }

    }
}

