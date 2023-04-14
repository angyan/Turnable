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
            FirstGid = this.FirstGid,
            Source = this.Source
        };
    }

    public  Location AtlasLocation(int tileGlobalId)
    {
        int tileId = tileGlobalId - this.FirstGid;
        int atlasX = tileId % this.Columns;
        int atlasY = tileId / this.Columns;

        return new(atlasX, atlasY);
    }
};
