namespace Turnable.Layouts;

public readonly record struct Coordinate
{
    public int Value { get; init; }

    internal Coordinate(int value)
    {
        Value = value;
    }

    public static implicit operator Coordinate(int value) => new(value);
    public static implicit operator int(Coordinate coordinate) => coordinate.Value; 
}