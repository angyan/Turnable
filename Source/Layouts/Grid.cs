using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Turnable.Layouts;

internal static class Grid
{
    internal static bool Contains(this Bounds bounds, Location location) =>
        location.X >= bounds.TopLeft.X && location.Y >= bounds.TopLeft.Y
                                      && location.X <= bounds.TopLeft.X + bounds.Width - 1
                                      && location.Y <= bounds.TopLeft.Y + bounds.Height - 1;
    internal static List<Location> GetNeighbors(this Bounds bounds, Location location) =>
        GetNeighbors(bounds, location, (_, _) => true);

    internal static List<Location> GetNeighbors(this Bounds bounds, Location location, Func<Bounds, Location, bool> includeFunc)
    {
        int[] xOffsets = { -1, 0, 1 };
        int[] yOffsets = { -1, 0, 1 };

        return (from xOffset in xOffsets
                from yOffset in yOffsets // Every combination of xOffsets and yOffsets
                where !(yOffset == 0 && xOffset == 0) // xOffset = 0 and yOffset = 0 is the location itself
                let neighborX = location.X + xOffset
                let neighborY = location.Y + yOffset
                where includeFunc(bounds, new Location(neighborX, neighborY))
                select new Location(neighborX, neighborY))
            .ToList();
    }

    internal static List<Location> GetContainedNeighbors(this Bounds bounds, Location location,
        Func<Bounds, Location, bool> includeFunc)
    {
        bool IncludeFuncAndContainsPredicate(Bounds boundsArgument, Location locationArgument) => boundsArgument.Contains(locationArgument) && includeFunc(boundsArgument, locationArgument);

        return bounds.GetNeighbors(location, IncludeFuncAndContainsPredicate);
    }

    internal static ImmutableList<Location> GetLocations(this Bounds bounds) =>
        (from x in Enumerable.Range(bounds.TopLeft.X, bounds.Width)
            from y in Enumerable.Range(bounds.TopLeft.Y, bounds.Height)
            select new Location(x, y))
        .ToImmutableList();

}