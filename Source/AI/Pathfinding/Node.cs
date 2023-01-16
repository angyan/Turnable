using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turnable.AI.Pathfinding
{
    internal struct Node
    {
        public Vector2 Location;
        public HashSet<Node> Neighbors;

        public Node(int x, int y) : this(new Vector2(x, y))
        {
        }

        public Node(Vector2 location) : this()
        {
            Location = location;
            Neighbors = new HashSet<Node>();
        }

        public static bool Equals(Node a, Node b) => a.Location == b.Location;
        public bool Equals(Node other) => Equals(this, other);
        public override bool Equals(object obj) => obj is Node && Equals(this, (Node)obj);
        public static bool operator ==(Node a, Node b) => Equals(a, b);
        public static bool operator !=(Node a, Node b) => !Equals(a, b);
        public override int GetHashCode() => Location.GetHashCode();
        public override string ToString() {
            string neighborsRepresentation = "None";

            if (Neighbors.Count > 0)
            {
                neighborsRepresentation = "[";
                for (var index = 0; index < Neighbors.Count; index ++)
                {
                    neighborsRepresentation += Neighbors.ElementAt(index).Location.ToString();
                    if (index != (Neighbors.Count - 1)) {
                        neighborsRepresentation += ", ";
                    }
                }
                neighborsRepresentation += "]";
            }

            return $"{{ Location: {Location}, Neighbors: {neighborsRepresentation} }}";
        }
    }
}
