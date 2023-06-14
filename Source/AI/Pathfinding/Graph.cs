using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

public record Graph(ImmutableDictionary<Location, ImmutableList<Location>> Value)
{
    public int Count => Value.Count;

    public ImmutableList<Location> this[Location location] => Value[location];

    double Heuristic(Location a, Location b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    private Path ReconstructPath(Dictionary<Location, Location> cameFrom, Location start, Location end)
    {
        // Path not found
        if (!cameFrom.ContainsKey(end))
        {
            return new Path(ImmutableList.Create<Location>());
        }

        Location current = end;
        List<Location> nodes = new();
        while (current != start)
        {
            nodes.Add(current);
            current = cameFrom[current];
        }

        nodes.Add(start);
        nodes.Reverse();

        return new Path(nodes.ToImmutableList());
    }

    internal Path FindPath(Location start, Location end)
    {
        Dictionary<Location, Location> cameFrom = new();
        Dictionary<Location, double> costSoFar = new();

        PriorityQueue<Location, double> frontier = new();
        frontier.Enqueue(start, 0);

        cameFrom[start] = start;
        costSoFar[start] = 0;

        while (frontier.Count > 0)
        {
            Location current = frontier.Dequeue();

            if (current == end)
            {
                break;
            }

            foreach (var next in Value[current])
            {
                double newCost = costSoFar[current] + 1;
                if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                {
                    costSoFar[next] = newCost;
                    double priority = newCost + Heuristic(next, end);
                    frontier.Enqueue(next, priority);
                    cameFrom[next] = current;
                }
            }
        }

        return ReconstructPath(cameFrom, start, end);
    }

    internal ImmutableList<Path> FindPaths(Location start, IImmutableList<Location> ends)
    {
        return (from end in ends
            let path = FindPath(start, end)
            orderby path.NodeCount
            select path).ToImmutableList();
    }
}
