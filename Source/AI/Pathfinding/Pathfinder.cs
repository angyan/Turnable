using System.Collections.Immutable;
using Turnable.Layouts;
using Turnable.Places;
using Turnable.TiledMap;

namespace Turnable.AI.Pathfinding;

public static class Pathfinder
{
    // Reference: https://www.redblobgames.com/pathfinding/a-star/implementation.html
    public static PathfinderFunc GetPathfinderFunc(this Map map, int layerIndex,
        CollisionMasks collisionMasks, bool allowDiagonal) =>
        map.GetGraph(layerIndex, collisionMasks, allowDiagonal).ConstructPathfinderFunc();

    private static PathfinderFunc ConstructPathfinderFunc(this Graph layerGraph)
    {
        double Heuristic(Location a, Location b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

        Path ReconstructPath(Dictionary<Location, Location> cameFrom, Location start, Location end)
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
                nodes .Add(current);
                current = cameFrom[current];
            }

            nodes .Add(start);
            nodes .Reverse();

            return new Path(nodes.ToImmutableList());
        }

        Path PathfinderFunc(Location start, Location end)
        {
            Dictionary<Location, Location> cameFrom = new();
            Dictionary<Location, double> costSoFar = new();
            Graph graph = layerGraph;

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
        }

        return PathfinderFunc;
    }
}
