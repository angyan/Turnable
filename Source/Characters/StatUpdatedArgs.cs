using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Characters
{
    internal struct StatUpdatedArgs
    {
        internal int NewValue { get; }

        internal StatUpdatedArgs(int newValue) : this() => NewValue = newValue;
    }
}
