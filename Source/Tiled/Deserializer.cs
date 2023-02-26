using System.Text.Json;
using Turnable.TiledMap;

namespace Turnable.Tiled;

public static class Deserializer
{
    internal static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // NOTE: MapJsonString can never have the value "null" which is the only JSON string that returns null when deserialized
    public static Map Deserialize(this MapJsonString mapJsonString) => JsonSerializer.Deserialize<Map>(mapJsonString, JsonSerializerOptions)!;

    // NOTE: TilesetJsonString can never have the value "null" which is the only JSON string that returns null when deserialized
    public static Tileset Deserialize(this TilesetJsonString tilesetJsonString) => JsonSerializer.Deserialize<Tileset>(tilesetJsonString, JsonSerializerOptions)!;

    public static Tileset DeserializeAndMerge(this Tileset tileset, TilesetJsonString tilesetJsonString)
    {
        Tileset deserializedTileset = JsonSerializer.Deserialize<Tileset>(tilesetJsonString, JsonSerializerOptions)!;

        return deserializedTileset with
        {
            FirstGid = tileset.FirstGid,
            Source = tileset.Source
        };
    }
}