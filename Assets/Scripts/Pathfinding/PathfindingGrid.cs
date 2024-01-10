using UnityEngine;

namespace Pathfinding
{
    public class PathfindingGrid : MonoBehaviour
    {
        [SerializeField] private LayerMask obstacleMask;

        [SerializeField] private Vector2 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private PathfindingNode[,] grid;

        private float NodeDiameter => nodeRadius * 2;
        private Vector2Int gridSize;

        private void Start()
        {
            int gridX = Mathf.RoundToInt(gridWorldSize.x / NodeDiameter);
            int gridY = Mathf.RoundToInt(gridWorldSize.y / NodeDiameter);

            gridSize = new Vector2Int(gridX, gridY);
            CreateGrid();
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
                    grid[x, y] = new PathfindingNode(isWalkable, worldPoint);
                }
            }
        }

        private Vector3 GetWorldBottomLeftVector => transform.position - Vector3.right * gridWorldSize.x * 0.5f -
                                                    Vector3.forward * gridWorldSize.y * 0.5f;

        public PathfindingNode GetNode(Vector3 worldPosition)
        {
            float percentX = (worldPosition.x + gridWorldSize.x * 0.5f) / gridWorldSize.x;
            float percentY = (worldPosition.z + gridWorldSize.y * 0.5f) / gridWorldSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((gridSize.x - 1) * percentX);
            int y = Mathf.RoundToInt((gridSize.y - 1) * percentY);

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
                Gizmos.color = node.isWalkable ? Color.green : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector3.one * (NodeDiameter - 0.1f));
            }
        }
    }
}