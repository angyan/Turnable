namespace Turnable.Layouts;

internal readonly record struct Dimension
{
    public int Value { get; init; }

    internal Dimension(int value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{value} is not a valid value for a Dimension; it has to be positive and greater than 0");

        Value = value;
    }

    public static implicit operator Dimension(int value) => new(value);
    public static implicit operator int(Dimension dimension) => dimension.Value; 

    private static bool IsValid(int value) => value > 0;
}