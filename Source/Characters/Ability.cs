using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.Characters
{
    internal readonly record struct Ability(string Name, string Description, Stat Stat);
}
