using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.TiledMap;

public record Layer(int[] Data, int Height, int Id, string Name, int Opacity, string Type, bool Visible,
    int Width,
    int X, int Y)
{
    internal Bounds Bounds() => new(new(0, 0), new(this.Width, this.Height));

    public int TileGid(Location location) => this.Data[location.X + location.Y * this.Width];

    internal ImmutableList<Location> Obstacles() =>
        (from location in this.Bounds().GetLocations()
            where this.TileGid(location) != 0
            select location)
        .ToImmutableList();
};