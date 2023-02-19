using System.Collections.Immutable;

namespace Turnable.Tiled;

internal record MapFilePath
{
    internal string Value { get; init; }
    internal ImmutableArray<string> SupportedExtensions = new[] { ".json", ".JSON", ".tmj", ".TMJ" }.ToImmutableArray();

    internal MapFilePath(string value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{Path.GetExtension(value)} is not a supported file extension for a Tiled Map");

        Value = value;
    }

    public static implicit operator MapFilePath(string value) => new(value);
    public static implicit operator string(MapFilePath mapFilePath) => mapFilePath.Value;

    private bool IsValid(string value) => SupportedExtensions.Contains(Path.GetExtension(value));
}