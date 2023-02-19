namespace Turnable.Layouts;

internal readonly record struct Bounds
{
    internal Location TopLeft { get; init; }
    internal Dimension Width { get; init; }
    internal Dimension Height { get; init; }

    internal Bounds(Location topLeft, Dimension width, Dimension height)
    {
        if (!IsValidLocation(topLeft)) throw new ArgumentException($"{topLeft} is not a valid Location for a Bounds");

        TopLeft = topLeft;
        Width = width;
        Height = height;
    }

    private static bool IsValidLocation(Location location) => location.X >= 0 && location.Y >= 0;
}

