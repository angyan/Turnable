namespace Turnable.Tiled;

internal record TilesetJsonString
{
    internal string Value { get; init; }

    internal TilesetJsonString(string value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{(value == null ? "null" : "\"null\"")} is not a valid value for constructing a JsonString");

        Value = value;
    }

    public static implicit operator TilesetJsonString(string value) => new TilesetJsonString(value);
    public static implicit operator string(TilesetJsonString tilesetJsonString) => tilesetJsonString.Value;

    private static bool IsValid(string value) => value != null && value != "null";
}