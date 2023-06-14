using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.TiledMap;

public record Layer(int[] Data, int Height, int Id, string Name, int Opacity, string Type, bool Visible,
    int Width,
    int X, int Y)
{
    internal Bounds Bounds => new(new(0, 0), new(Width, Height));

    public int TileGid(Location location) => Data[location.X + location.Y * Width];

    internal ImmutableList<Location> GetObstacles() =>
        (from location in Bounds.GetLocations()
            where TileGid(location) != 0
            select location)
        .ToImmutableList();
};