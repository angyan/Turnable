using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.Layouts;

internal readonly record struct Bounds
{
    internal int TopLeftX { get; init; }
    internal int TopLeftY { get; init; }
    internal int Width { get; init; }
    internal int Height { get; init; }

    internal Bounds(int topLeftX, int topLeftY, int width, int height)
    {
        if (!IsValidDimensions(width, height)) throw new ArgumentException($"A width of {width} and a height of {height} are not valid dimensions for a bounds");
        if (!IsValidPosition(topLeftX, topLeftY)) throw new ArgumentException($"{topLeftX}, {topLeftY} are not valid coordinates for the top left position of a bounds");

        TopLeftX = topLeftX;
        TopLeftY = topLeftY;
        Width = width;
        Height = height;
    }

    private static bool IsValidDimensions(int width, int height) => width > 0 && height > 0;
    private static bool IsValidPosition(int x, int y) => x >= 0 && y >= 0;
}

