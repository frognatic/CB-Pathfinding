using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingNode
    {
        public bool IsWalkable { get; }
        public Vector3 WorldPosition { get; }
        public Vector2Int GridPosition { get; }
        public int GCost { get; private set; }
        public int HCost { get; private set; }
        public int FCost => GCost + HCost;

        private PathfindingNode previousNode;
        public PathfindingNode PreviousNode => previousNode;
        public List<PathfindingNode> NeighboursList { get; } = new();

        public string ID => $"Grid position [{GridPosition.x} {GridPosition.y}] || World position [{WorldPosition.x} {WorldPosition.y} {WorldPosition.z}]";

        public PathfindingNode(bool isWalkable, Vector3 worldPosition, Vector2Int gridPosition)
        {
            IsWalkable = isWalkable;
            WorldPosition = worldPosition;
            GridPosition = gridPosition;
        }

        public void AddNeighbour(PathfindingNode neighbour) => NeighboursList.Add(neighbour);

        public void SetGCost(int valueToSet) => GCost = valueToSet;
        public void SetHCost(int valueToSet) => HCost = valueToSet;
        public void SetPreviousNode(PathfindingNode node) => previousNode = node;
    }
}