using System.Collections.Immutable;
using Turnable.Layouts;

namespace Turnable.AI.Pathfinding;

internal static class Pathfinder
{
    // Reference: https://www.redblobgames.com/pathfinding/a-star/implementation.html
    internal static Func<Location, Location, ImmutableList<Location>> GetPathfinder(this Graph layerGraph)
    {
        double Heuristic(Location a, Location b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        ImmutableList<Location> ReconstructPath(Dictionary<Location, Location> cameFrom, Location start, Location end)
        {
            // Path not found
            if (!cameFrom.ContainsKey(end))
            {
                return ImmutableList.Create<Location>();
            }

            Location current = end;
            List<Location> path = new();
            while (current != start)
            {
                path.Add(current);
                current = cameFrom[current];
            }

            path.Add(start);
            path.Reverse();

            return path.ToImmutableList();
        }

        return (start, end) =>
        {
            Dictionary<Location, Location> cameFrom = new();
            Dictionary<Location, double> costSoFar = new();
            ImmutableDictionary<Location, ImmutableList<Location>> graph = layerGraph;

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

                foreach (var next in graph[current])
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
        };
    }
}
