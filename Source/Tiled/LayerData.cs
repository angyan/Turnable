using Turnable.Layouts;
using Turnable.TiledMap;

namespace Turnable.Tiled;

internal static class LayerData
{
    internal static int TileGidAt(this Layer layer, Location location)
    {
        return layer.Data[location.X + location.Y * layer.Width];
    }
}