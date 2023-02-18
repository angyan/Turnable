using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.Layouts;

internal readonly record struct GridLocation
{
    public int X { get; init; }
    public int Y { get; init; }

    internal GridLocation(int x, int y, Bounds bounds)
    {
        if (!IsValid(x, y, bounds)) throw new ArgumentException($"{new Location(x, y)} is not a valid location within {bounds}");

        X = x;
        Y = y;
    }

    internal GridLocation(Location location, Bounds bounds) : this(location.X, location.Y, bounds) { }

    private static bool IsValid(int x, int y, Bounds bounds) => bounds.Contains(x, y);
}
