using FluentAssertions;
using Turnable.Tiled;

namespace Tests.Tiled;

public class MapFilePathTests
{
    [Theory]
    [InlineData("")]
    [InlineData(".tmx")]
    [InlineData(".TMX")]
    [InlineData(".txt")]
    internal void Map_file_path_cannot_be_constructed_without_specifying_the_correct_extension(string extension)
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
    internal void Map_file_path_can_only_be_constructed_with_supported_file_extensions(string extension)
    {
        // No arrange

        Action construction = () => new MapFilePath($"map{extension}");

        construction.Should().NotThrow<ArgumentException>();
    }

    [Fact]
    internal void Map_file_path_can_be_implicitly_converted_to_a_string()
    {
        MapFilePath mapFilePath = new("map.tmj");

        string filePath = mapFilePath;

        filePath.Should().Be("map.tmj");
    }

    [Fact] internal void String_can_be_implicitly_converted_to_a_map_file_path()
    {
        string filePath = "map.tmj";

        MapFilePath mapFilePath = filePath;

        mapFilePath.Value.Should().Be("map.tmj");
    }
}