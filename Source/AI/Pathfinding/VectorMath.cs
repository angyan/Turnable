using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Tests")]

namespace Turnable.AI.Pathfinding;

internal static class VectorMath
{
    internal static Vector2 Add(this Vector2 a, Vector2 b) => new(a.X + b.X, a.Y + b.Y);
    internal static Vector2 Negate(this Vector2 a) => new(-a.X, -a.Y);
    internal static Vector2 Subtract(this Vector2 a, Vector2 b) => new(a.X - b.X, a.Y - b.Y);
    internal static double Magnitude(this Vector2 a) => Math.Sqrt(a.X * a.X + a.Y * a.Y);
    internal static double Distance(this Vector2 a, Vector2 b) => Subtract(a, b).Magnitude();
}

