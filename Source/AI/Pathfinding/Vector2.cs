using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.AI.Pathfinding;

internal readonly record struct Vector2(int X, int Y);
