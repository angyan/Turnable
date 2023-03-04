using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

public delegate Path PathfinderFunc(Location start, Location end);
