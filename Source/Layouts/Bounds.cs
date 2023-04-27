using System.Collections.Immutable;

namespace Turnable.Layouts;

internal record Bounds
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

    internal static ImmutableList<Location> GetNeighbors(Bounds bounds, Location location, bool allowDiagonal,
        Func<Location, bool> includeLocationPredicateFunc)
    {
        int[] xOffsets = { -1, 0, 1 };
        int[] yOffsets = { -1, 0, 1 };

        Func<Location, bool> includeNeighborLocationFunc = allowDiagonal
            // If we allow diagonal movement, we need to include all locations that are within bounds and pass the predicate function
            ? (Location locationArgument) =>
                bounds.Contains(locationArgument) && includeLocationPredicateFunc(locationArgument)
            // If we don't allow diagonal movement, we need to include all locations that are within bounds, on the same row or column as the location, and pass the predicate function
            : (Location locationArgument) =>
                bounds.Contains(locationArgument) &&
                (locationArgument.X == location.X || locationArgument.Y == location.Y) &&
                includeLocationPredicateFunc(locationArgument);

        return (from xOffset in xOffsets
                from yOffset in yOffsets // Every combination of xOffsets and yOffsets
                where !(yOffset == 0 && xOffset == 0) // xOffset = 0 and yOffset = 0 is the location itself
                let neighborLocation = new Location(location.X + xOffset, location.Y + yOffset)
                where includeNeighborLocationFunc(neighborLocation)
                select neighborLocation)
            .Distinct().ToImmutableList();
    }

    internal ImmutableList<Location> GetLocations(Func<Location, bool> includeFunc)  {
        Bounds bounds = this;

        return
            (from x in Enumerable.Range(bounds.TopLeft.X, bounds.Dimensions.Width)
                from y in Enumerable.Range(bounds.TopLeft.Y, bounds.Dimensions.Height)
                where includeFunc(new Location(x, y))
                select new Location(x, y))
            .ToImmutableList();
    }

    internal ImmutableList<Location> GetLocations() => GetLocations(_ => true);

    internal ImmutableList<Location> GetLocations(Location from, int distance, bool allowDiagonal)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            from.DistanceTo(locationArgument, allowDiagonal) == distance;

        return GetLocations(IncludeLocationPredicateFunc);
    }

    internal ImmutableList<Location> GetLocations(Location from, int startDistance, int endDistance, bool allowDiagonal)
    {
        bool IncludeLocationPredicateFunc(Location locationArgument) =>
            from.DistanceTo(locationArgument, allowDiagonal) >= startDistance &&
            from.DistanceTo(locationArgument, allowDiagonal) <= endDistance;

        return GetLocations(IncludeLocationPredicateFunc);
    }

    internal int GetLocationCount(Location from, int distance, bool allowDiagonal) => GetLocations(from, distance, allowDiagonal).Count;
}