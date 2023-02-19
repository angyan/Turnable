using System.Collections.Immutable;
using System.ComponentModel;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

internal record PathfinderContext(ImmutableDictionary<Location, ImmutableList<Location>> Graph);
