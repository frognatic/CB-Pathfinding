using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using Pathfinding;
using UnityEngine;

namespace Managers
{
    public class PathfindingManager : MonoSingleton<PathfindingManager>
    {
        private PathfindingGrid grid;
        public PathfindingGrid Grid => grid;
        
        private const int MoveDiagonalCost = 14;
        private const int MoveStraightCost = 10;

        private List<PathfindingNode> openSet;
        private HashSet<PathfindingNode> closedSet;
        
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Pathfinding, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Pathfinding);
            InitGrid();
        }

        private void InitGrid()
        {
            grid = PathfindingGridHolder.Holder.GetComponent<PathfindingGrid>();
            grid.InitializeGrid();
        }

        public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos)
        {
            PathfindingNode startNode = grid.GetNode(startPos);
            PathfindingNode targetNode = grid.GetNode(targetPos);

            openSet = new List<PathfindingNode>();
            closedSet = new HashSet<PathfindingNode>();
            
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathfindingNode currentNode = openSet.First();

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].FCost < currentNode.FCost || openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost)
                        currentNode = openSet[i];
                }

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    return CalculatedPath(startNode, targetNode);
                }
                
                CalculateMoveToNeighbourCosts(currentNode, targetNode);
            }

            return null;
        }

        private void CalculateMoveToNeighbourCosts(PathfindingNode currentNode, PathfindingNode targetNode)
        {
            foreach (PathfindingNode neighbour in currentNode.NeighboursList)
            {
                if (!neighbour.IsWalkable || closedSet.Contains(neighbour))
                    continue;

                int costMovingToNeighbour = currentNode.GCost + CalculateDistanceCost(currentNode, neighbour);
                if (costMovingToNeighbour >= neighbour.GCost && openSet.Contains(neighbour)) 
                    continue;
                
                neighbour.SetGCost(costMovingToNeighbour);
                neighbour.SetHCost(CalculateDistanceCost(neighbour, targetNode));
                neighbour.SetPreviousNode(currentNode);

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }
        
        private List<Vector3> CalculatedPath(PathfindingNode startNode, PathfindingNode endNode)
        {
            List<PathfindingNode> path = new List<PathfindingNode>();
            PathfindingNode currentNode = endNode;
            
            while (currentNode != startNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.PreviousNode;
            }

            path.Reverse();

            List<Vector3> vectorPath = new List<Vector3>();
            foreach (var node in path) 
                vectorPath.Add(node.WorldPosition);

            return vectorPath;
        }

        private int CalculateDistanceCost(PathfindingNode first, PathfindingNode second)
        {
            int xDistance = Mathf.Abs(first.GridPosition.x - second.GridPosition.x);
            int yDistance = Mathf.Abs(first.GridPosition.y - second.GridPosition.y);
            int distanceDiff = Mathf.Abs(xDistance - yDistance);
            
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * distanceDiff;
        }
    }
}
