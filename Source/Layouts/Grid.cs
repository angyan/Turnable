using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.Layouts;

internal static class Grid
{
    internal static bool Contains(this Bounds bounds, Location location) =>
        Contains(bounds, location.X, location.Y);

    internal static bool Contains(this Bounds bounds, int x, int y) =>
        x >= bounds.TopLeftX && y >= bounds.TopLeftY
                             && x <= bounds.TopLeftX + bounds.Width - 1
                             && y <= bounds.TopLeftY + bounds.Height - 1;

    internal static List<Location> NeighborsFor(this Bounds bounds, GridLocation gridLocation) =>
        NeighborsFor(bounds, gridLocation, (_, _) => true);

    internal static List<Location> NeighborsFor(this Bounds bounds, GridLocation gridLocation, Func<Bounds, Location, bool> includeFunc)
    {
        int[] xOffsets = { -1, 0, 1 };
        int[] yOffsets = { -1, 0, 1 };

        return (from xOffset in xOffsets
                from yOffset in yOffsets // Every combination of xOffsets and yOffsets
                where !(yOffset == 0 && xOffset == 0) // xOffset = 0 and yOffset = 0 is the location itself
                let neighborX = gridLocation.X + xOffset
                let neighborY = gridLocation.Y + yOffset
                where includeFunc(bounds, new Location(neighborX, neighborY))
                select new Location(neighborX, neighborY))
            .ToList();
    }
}