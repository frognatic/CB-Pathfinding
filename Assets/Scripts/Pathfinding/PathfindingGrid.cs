using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
    public class PathfindingGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleMask;

        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private PathfindingNode[,] grid;

        public float NodeRadius => nodeRadius;
        
        private float NodeDiameter => nodeRadius * 2;
        private Vector2Int gridSize;
        
        public void InitializeGrid()
        {
            int gridX = Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
            int gridY = Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);

            gridSize = new Vector2Int(gridX, gridY);
            CreateGrid();
            FillAllNeighbours();
        }

        private void CreateGrid()
        {
            grid = new PathfindingNode[gridSize.x, gridSize.y];
            Vector3 worldBottomLeftVector = GetWorldBottomLeftVector;

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    Vector3 worldPoint = worldBottomLeftVector + Vector3.right * (x * NodeDiameter + nodeRadius) +
                                         Vector3.forward * (y * NodeDiameter + nodeRadius);
                    bool isWalkable = !Physics.CheckSphere(worldPoint, nodeRadius, obstacleMask);

                    Vector2Int gridPosition = new Vector2Int(x, y);
                    grid[x, y] = new PathfindingNode(isWalkable, worldPoint, gridPosition);
                }
            }
        }

        private void FillAllNeighbours()
        {
            foreach (var node in grid)
                FillNeighboursList(node);
        }

        private void FillNeighboursList(PathfindingNode node)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) 
                        continue;

                    int checkX = node.GridPosition.x + x;
                    int checkY = node.GridPosition.y + y;

                    if (checkX >= 0 && checkX < gridSize.x && checkY >= 0 && checkY < gridSize.y)
                        node.AddNeighbour(grid[checkX, checkY]);
                }
            }
        }

        private Vector3 GetWorldBottomLeftVector => transform.position - Vector3.right * gridWorldSize.x * 0.5f -
                                                    Vector3.forward * gridWorldSize.y * 0.5f;

        public PathfindingNode GetNode(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;

            int x = Mathf.FloorToInt(Mathf.Clamp(gridSize.x * percentX, 0, gridSize.x - 1));
            int y = Mathf.RoundToInt(Mathf.Clamp(gridSize.y * percentY + 1, 0, gridSize.y - 1));
            
            return grid[x, y];
        }

        private void OnDrawGizmos()
        {
#if DRAW_PATHFINDING_GIZMOS
            DrawGridGizmos();
            DrawNodeGizmos();
#endif
        }

        private void DrawGridGizmos()
        {
            Gizmos.color = Color.blue;

            Vector3 size = new Vector3(gridWorldSize.x, 1, gridWorldSize.y);
            Gizmos.DrawWireCube(transform.position, size);
        }


        private void DrawNodeGizmos()
        {
            if (grid == null) return;

            foreach (PathfindingNode node in grid)
            {
                Gizmos.color = node.IsWalkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.WorldPosition, Vector3.one * (NodeDiameter - 0.1f));
            }
        }
    }
}