using System.Collections.Immutable;

namespace Turnable.Tiled;

public record TilesetFilePath
{
    internal string Value { get; init; }
    internal ImmutableArray<string> SupportedExtensions = new[] { ".json", ".JSON", ".tsj", ".TSJ" }.ToImmutableArray();

    public TilesetFilePath(string value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{Path.GetExtension(value)} is not a supported file extension for a Tiled Tileset");

        Value = value;
    }

    public static implicit operator string(TilesetFilePath tilesetFilePath) => tilesetFilePath.Value;

    private bool IsValid(string value) => SupportedExtensions.Contains(Path.GetExtension(value));
}