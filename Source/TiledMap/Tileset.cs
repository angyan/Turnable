using System.Text.Json;
using Turnable.Layouts;
using Turnable.Tiled;

namespace Turnable.TiledMap;

public record Tileset(
    int Columns, int FirstGid, string Image, int ImageHeight, int ImageWidth, int Margin,
    string Name, string Source, int Spacing,
    int TileCount, string TiledVersion, int TileHeight, int TileWidth, string Type, string Version)
{
    public Tileset DeserializeAndMerge(TilesetJsonString tilesetJsonString)
    {
        Tileset deserializedTileset = JsonSerializer.Deserialize<Tileset>(tilesetJsonString, new JsonSerializerOptions()
            { PropertyNameCaseInsensitive = true })!;

        return deserializedTileset with
        {
            FirstGid = FirstGid,
            Source = Source
        };
    }

    public  Location AtlasLocation(int tileGlobalId)
    {
        int tileId = tileGlobalId - FirstGid;
        int atlasX = tileId % Columns;
        int atlasY = tileId / Columns;

        return new(atlasX, atlasY);
    }
};
