using System.Xml.Serialization;

namespace Turnable.TiledMap;

public record Tileset(
    int Columns, int FirstGid, string Image, int ImageHeight, int ImageWidth, int Margin,
    string Name, string Source, int Spacing,
    int TileCount, string TiledVersion, int TileHeight, int TileWidth, string Type, string Version);