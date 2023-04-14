using System.Collections.Immutable;

namespace Turnable.Layouts;

internal readonly record struct Bounds
{
    internal Location TopLeft { get; init; }
    internal Size Dimensions { get; init; }

    internal Bounds(Location topLeft, Size dimensions)
    {
        if (!IsValid(topLeft)) throw new ArgumentException($"{topLeft} is not a valid Location for a Bounds");

        TopLeft = topLeft;
        Dimensions = dimensions;
    }

    private static bool IsValid(Location location) => location.X >= 0 && location.Y >= 0;

    internal bool Contains(Location location) =>
        location.X >= TopLeft.X && location.Y >= TopLeft.Y
                                && location.X <= TopLeft.X + Dimensions.Width - 1
                                && location.Y <= TopLeft.Y + Dimensions.Height - 1;

    internal static ImmutableList<Location> GetNeighbors(Bounds bounds, Location location, Func<Location, bool> includeLocationPredicateFunc)
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

    internal static ImmutableList<Location> GetNeighbors(Bounds bounds, Location location) =>
        GetNeighbors(bounds, location, _ => true);

    internal static ImmutableList<Location> GetContainedNeighbors(Bounds bounds, Location location)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) => bounds.Contains(locationArgument);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNeighbors(Bounds bounds, Location location,
        Func<Location, bool> includeFunc)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) && includeFunc(locationArgument);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNonDiagonalNeighbors(Bounds bounds, Location location)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) && (locationArgument.X == location.X || locationArgument.Y == location.Y);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal static ImmutableList<Location> GetContainedNonDiagonalNeighbors(Bounds bounds, Location location,
        Func<Location, bool> includeFunc)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            bounds.Contains(locationArgument) &&
            (locationArgument.X == location.X || locationArgument.Y == location.Y) && includeFunc(locationArgument);

        return GetNeighbors(bounds, location, IncludeLocationPredicateFunc);
    }

    internal ImmutableList<Location> GetLocations()  {
        Bounds bounds = this;

        return
            (from x in Enumerable.Range(bounds.TopLeft.X, bounds.Dimensions.Width)
                from y in Enumerable.Range(bounds.TopLeft.Y, bounds.Dimensions.Height)
                select new Location(x, y))
            .ToImmutableList();
    }
}

