using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Characters
{
    internal struct StatClampedArgs
    {
        internal int TriedValue { get; }

        internal StatClampedArgs(int triedValue) : this() => TriedValue = triedValue;
    }
}
