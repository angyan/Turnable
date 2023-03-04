namespace Turnable.Tiled;

public record MapJsonString
{
    internal string Value { get; init; }

    public MapJsonString(string value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{(value == null ? "null" : "\"null\"")} is not a valid value for constructing a JsonString");

        Value = value;
    }

    public static implicit operator string(MapJsonString mapJsonString) => mapJsonString.Value;

    private bool IsValid(string value) => value != null && value != "null";
}