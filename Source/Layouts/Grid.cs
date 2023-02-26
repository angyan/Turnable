using System.Collections.Immutable;

namespace Turnable.Layouts;

internal static class Grid
{
    internal static bool Contains(this Bounds bounds, Location location) =>
        location.X >= bounds.TopLeft.X && location.Y >= bounds.TopLeft.Y
                                      && location.X <= bounds.TopLeft.X + bounds.Width - 1
                                      && location.Y <= bounds.TopLeft.Y + bounds.Height - 1;
    internal static ImmutableList<Location> GetNeighbors(this Bounds bounds, Location location, Func<Location, bool> includeLocationPredicateFunc)
    {
        int[] xOffsets = { -1, 0, 1 };
        int[] yOffsets = { -1, 0, 1 };

        return (from xOffset in xOffsets
                from yOffset in yOffsets // Every combination of xOffsets and yOffsets
                where !(yOffset == 0 && xOffset == 0) // xOffset = 0 and yOffset = 0 is the location itself
                let neighborX = location.X + xOffset
                let neighborY = location.Y + yOffset
                where includeLocationPredicateFunc(new Location(neighborX, neighborY))
                select new Location(neighborX, neighborY))
            .ToImmutableList();
    }

    internal static ImmutableList<Location> GetNeighbors(this Bounds bounds, Location location) =>
        GetNeighbors(bounds, location, _ => true);

    internal static ImmutableList<Location> GetContainedNeighbors(this Bounds bounds, Location location)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) => bounds.Contains(locationArgument);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNeighbors(this Bounds bounds, Location location,
        Func<Location, bool> includeFunc)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) && includeFunc(locationArgument);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNonDiagonalNeighbors(this Bounds bounds, Location location)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) && (locationArgument.X == location.X || locationArgument.Y == location.Y);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNonDiagonalNeighbors(this Bounds bounds, Location location,
        Func<Location, bool> includeFunc)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) && includeFunc(locationArgument) &&
            (locationArgument.X == location.X || locationArgument.Y == location.Y);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetLocations(this Bounds bounds) =>
        (from x in Enumerable.Range(bounds.TopLeft.X, bounds.Width)
            from y in Enumerable.Range(bounds.TopLeft.Y, bounds.Height)
            select new Location(x, y))
        .ToImmutableList();

}