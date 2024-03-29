﻿using System.Text.Json;
using Turnable.TiledMap;

namespace Turnable.Tiled;

public record TilesetJsonString
{
    internal string Value { get; init; }

    public TilesetJsonString(string value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{(value == null ? "null" : "\"null\"")} is not a valid value for constructing a JsonString");

        Value = value;
    }

    public static implicit operator string(TilesetJsonString tilesetJsonString) => tilesetJsonString.Value;

    private static bool IsValid(string value) => value != null && value != "null";

    // NOTE: TilesetJsonString can never have the value "null" which is the only JSON string that returns null when deserialized
    public Tileset Deserialize() =>
        JsonSerializer.Deserialize<Tileset>(Value,
            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;
}