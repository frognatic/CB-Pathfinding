using UnityEngine;

namespace Pathfinding
{
    public class PathfindingNode
    {
        public bool isWalkable;
        public Vector3 worldPosition;

        public PathfindingNode(bool isWalkable, Vector3 worldPosition)
        {
            this.isWalkable = isWalkable;
            this.worldPosition = worldPosition;
        }
    }
}
