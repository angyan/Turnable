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
        internal int Min { get; init; }
        internal int Max { get; init; }

        internal Stat(int value, int min, int max)
        {
            if (!IsValid(value, min, max)) throw new ArgumentException($"{value} is not a valid value for a Stat that has a Min of {min} and a Max of {max}");

            Value = value;
            Min = min;
            Max = max;
        }

        public event Action<object, StatClampedArgs>? MinReached;
        public event Action<object, StatClampedArgs>? MaxReached;

        internal void RaiseMinReached(int triedValue) => MinReached?.Invoke(this, new StatClampedArgs(triedValue));
        internal void RaiseMaxReached(int triedValue) => MaxReached?.Invoke(this, new StatClampedArgs(triedValue));
        
        private static bool IsValid(int value, int min, int max) => value >= min && value <= max;
    }
}
