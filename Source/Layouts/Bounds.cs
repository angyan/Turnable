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
}

