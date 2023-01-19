using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Tests")]
namespace Turnable.AI.Pathfinding
{
    internal readonly struct Vector2 : IEquatable<Vector2>
    {
        internal int X { get; }
        internal int Y { get; }
        internal static readonly Vector2 Zero = default;

        internal Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }
        internal static bool Equals(Vector2 a, Vector2 b) => a.X == b.X && a.Y == b.Y;
        
        public bool Equals(Vector2 other) => Equals(this, other);
        
        public override bool Equals(object obj) => obj is Vector2 && Equals(this, (Vector2)obj);
        
        public static bool operator ==(Vector2 a, Vector2 b) => Equals(a, b);
        
        public static bool operator !=(Vector2 a, Vector2 b) => !(a == b);
        
        public override int GetHashCode() => X + 17 * Y;
        
        public override string ToString() => $"({X}, {Y})";
        
        public static Vector2 operator +(Vector2 a, Vector2 b) => new Vector2(a.X + b.X, a.Y + b.Y);
        
        public static Vector2 operator -(Vector2 a) => new Vector2(-a.X, -a.Y);
        
        public static Vector2 operator -(Vector2 a, Vector2 b) => new Vector2(a.X - b.X, a.Y - b.Y);

        internal double Magnitude => Math.Sqrt(X * X + Y * Y);

        internal static double Distance(Vector2 a, Vector2 b) => (a - b).Magnitude;

        internal double Distance(Vector2 a) => Distance(this, a);
    }
}
