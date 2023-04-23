namespace Turnable.Layouts;

public record Measurement
{
    public int Value { get; init; }

    internal Measurement(int value)
    {
        if (!IsValid(value)) throw new ArgumentException($"{value} is not a valid value for a Dimension; it has to be positive and greater than 0");

        Value = value;
    }

    public static implicit operator Measurement(int value) => new(value);
    public static implicit operator int(Measurement dimension) => dimension.Value; 

    private static bool IsValid(int value) => value > 0;
}