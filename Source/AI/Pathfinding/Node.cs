using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Tests")]
namespace Turnable.AI.Pathfinding
{
    internal readonly struct Node
    {
        internal Vector2 Location { get; }
        internal HashSet<Node> Neighbors { get; }

        internal Node(int x, int y) : this(new Vector2(x, y))
        {
        }

        internal Node(Vector2 location) : this()
        {
            Location = location;
            Neighbors = new HashSet<Node>();
        }

        internal static bool Equals(Node a, Node b) => a.Location == b.Location;

        internal bool Equals(Node other) => Equals(this, other);

        public override bool Equals(object obj) => obj is Node && Equals(this, (Node)obj);

        public static bool operator ==(Node a, Node b) => Equals(a, b);

        public static bool operator !=(Node a, Node b) => !Equals(a, b);
        
        public override int GetHashCode() => Location.GetHashCode();

        public override string ToString() {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append("None");

            if (Neighbors.Count <= 0) return $"{{ Location: {Location}, Neighbors: {stringBuilder} }}";

            stringBuilder.Clear();
            stringBuilder.Append('[');
            for (var index = 0; index < Neighbors.Count; index ++)
            {
                stringBuilder.Append(Neighbors.ElementAt(index).Location.ToString());
                if (index != (Neighbors.Count - 1)) {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append(']');

            return $"{{ Location: {Location}, Neighbors: {stringBuilder} }}";
        }
    }
}
