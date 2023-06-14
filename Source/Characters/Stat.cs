namespace Turnable.Characters
{
    internal record Stat
    {
        internal int Value { get; init; }
        internal int Minimum { get; init; }
        internal int Maximum { get; init; }

        internal Stat(int value, int minimum, int maximum)
        {
            if (!IsValid(value, minimum, maximum)) throw new ArgumentException($"{value} is not a valid value for a Stat that has a Min of {minimum} and a Max of {maximum}");

            Value = value;
            Minimum = minimum;
            Maximum = maximum;
        }

        public event Action<object, StatClampedArgs>? MinimumReached;
        public event Action<object, StatClampedArgs>? MaximumReached;
        public event Action<object, StatUpdatedArgs>? ValueUpdated;
        
        private static bool IsValid(int value, int minimum, int maximum) => value >= minimum && value <= maximum;

        internal Stat Update(int newValue)
        {
            if (newValue <= Minimum)
            {
                MinimumReached?.Invoke(this, new StatClampedArgs(newValue));
                return this with { Value = Minimum };
            }
            else if (newValue >= Maximum)
            {
                MaximumReached?.Invoke(this, new StatClampedArgs(newValue));
                return this with { Value = Maximum };
            }
            else if (newValue != Value)
            {
                ValueUpdated?.Invoke(this, new StatUpdatedArgs(newValue));
                return this with { Value = newValue };
            }
            else
            {
                return this;
            }
        }
    }
}
