using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        internal void RaiseMinimumReached(int triedValue) => MinimumReached?.Invoke(this, new StatClampedArgs(triedValue));
        internal void RaiseMaximumReached(int triedValue) => MaximumReached?.Invoke(this, new StatClampedArgs(triedValue));
        internal void RaiseValueUpdated(int newValue) => ValueUpdated?.Invoke(this, new StatUpdatedArgs(newValue));
        
        private static bool IsValid(int value, int minimum, int maximum) => value >= minimum && value <= maximum;
    }
}
