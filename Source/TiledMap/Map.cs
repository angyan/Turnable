namespace Turnable.TiledMap;

public record Map(int CompressionLevel, int Height, bool Infinite, Layer[] Layers, int NextLayerId,
    int NextObjectId,
    string Orientation, string RenderOrder, string TiledVersion, int TileHeight, Tileset[] Tilesets, int TileWidth,
    string Type, string Version, int Width);