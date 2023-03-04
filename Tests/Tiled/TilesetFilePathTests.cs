using FluentAssertions;
using Turnable.Tiled;

namespace Tests.Tiled;

public class TilesetFilePathTests
{
    [Theory]
    [InlineData("")]
    [InlineData(".tsx")]
    [InlineData(".TSX")]
    [InlineData(".txt")]
    internal void Tileset_file_path_cannot_be_constructed_without_specifying_the_correct_extension(string extension)
    {
        // No arrange

        Action construction = () => new TilesetFilePath($"tileset{extension}");

        construction.Should().Throw<ArgumentException>().WithMessage($"{extension} is not a supported file extension for a Tiled Tileset");
    }

    [Theory]
    [InlineData(".tsj")]
    [InlineData(".TSJ")]
    [InlineData(".json")]
    [InlineData(".JSON")]
    internal void Tileset_file_path_can_only_be_constructed_with_supported_file_extensions(string extension)
    {
        // No arrange

        Action construction = () => new TilesetFilePath($"tileset{extension}");

        construction.Should().NotThrow<ArgumentException>();
    }

    [Fact]
    internal void Tileset_file_path_can_be_implicitly_converted_to_a_string()
    {
        TilesetFilePath tilesetFilePath = new("tileset.tsj");

        string filePath = tilesetFilePath;

        filePath.Should().Be("tileset.tsj");
    }
}