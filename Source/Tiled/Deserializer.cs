using System.Text.Json;
using Turnable.TiledMap;

namespace Turnable.Tiled;

internal static class Deserializer
{
    internal static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // NOTE: MapJsonString can never have the value "null" which is the only JSON string that returns null when deserialized
    internal static Map Deserialize(this MapJsonString mapJsonString) => JsonSerializer.Deserialize<Map>(mapJsonString, JsonSerializerOptions)!;

    // NOTE: TilesetJsonString can never have the value "null" which is the only JSON string that returns null when deserialized
    internal static Tileset Deserialize(this TilesetJsonString tilesetJsonString) => JsonSerializer.Deserialize<Tileset>(tilesetJsonString, JsonSerializerOptions)!;

    internal static Tileset DeserializeAndMerge(this Tileset tileset, TilesetJsonString tilesetJsonString)
    {
        Tileset deserializedTileset = JsonSerializer.Deserialize<Tileset>(tilesetJsonString, JsonSerializerOptions)!;

        return new Tileset(
            deserializedTileset.Columns,
            tileset.FirstGid,
            deserializedTileset.Image,
            deserializedTileset.ImageHeight,
            deserializedTileset.ImageWidth,
            deserializedTileset.Margin,
            deserializedTileset.Name,
            tileset.Source,
            deserializedTileset.Spacing,
            deserializedTileset.TileCount,
            deserializedTileset.TiledVersion,
            deserializedTileset.TileHeight,
            deserializedTileset.TileWidth,
            deserializedTileset.Type,
            deserializedTileset.Version
        );
    }
}