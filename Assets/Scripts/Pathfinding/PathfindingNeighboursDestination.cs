using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingNeighboursDestination
    {
        private PathfindingNode hitNode;
        private readonly HashSet<PathfindingNode> nodes;
        private readonly int amount;
        private int neighbour;

        public PathfindingNeighboursDestination(PathfindingNode hitNode, int amount)
        {
            this.hitNode = hitNode;
            this.amount = amount;

            neighbour = -1;
            nodes = new();
        }
    
        private void FillNeighboursToLeaderDestination()
        {
            while (true)
            {
                neighbour++;
                foreach (PathfindingNode node in hitNode.NeighboursList)
                {
                    if (node.IsWalkable) 
                        nodes.Add(node);

                    if (nodes.Count >= amount)
                        break;
                }

                if (nodes.Count < amount)
                {
                    hitNode = nodes.ToList()[neighbour];
                    continue;
                }

                break;
            }
        }

        public List<Vector3> GetDestinations()
        {
            FillNeighboursToLeaderDestination();
            List<Vector3> list = nodes.Select(x => x.WorldPosition).ToList();

            return list;
        }
    }
}
