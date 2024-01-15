using System.Collections.Generic;
using Managers;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsMono : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private GameObject leaderCrown;
        
        private MovingUnit movingUnit;

        private int currentPathIndex;
        private List<Vector3> pathVectorList;

        private float unitHeight;
        private LayerMask terrainLayerMask;

        private float GridRadius => PathfindingManager.Instance.Grid.NodeRadius;

        public void Init(MovingUnit movingUnit)
        {
            this.movingUnit = movingUnit;
            
            SetPosition();
            SetUnitColor();
        }
        
        private void SetPosition() => transform.position = movingUnit.Position;

        private void SetUnitColor()
        {
            UnitDetails unitDetails = AddressableManager.Instance.GetUnitDetails($"unit_{movingUnit.Id}");
            
            MaterialPropertyBlock materialPropertyBlock = new();
            materialPropertyBlock.SetColor("_Color", unitDetails.color);
            
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        private void Awake()
        {
            unitHeight = transform.position.y;
            terrainLayerMask = LayerMask.NameToLayer("Terrain");
        }

        private void Update()
        {
            if (!CanUnitMove())
                return;

            HandleMovement();
        }

        private bool CanUnitMove() => PathfindingManager.Instance.Grid != null;

        private void HandleMovement()
        {
            if (pathVectorList == null) 
                return;
            
            Vector3 targetPosition = pathVectorList[currentPathIndex];
            targetPosition.y = unitHeight;
            if (Vector3.Distance(transform.position, targetPosition) > GridRadius)
            {
                Vector3 position = transform.position;
                Vector3 moveDir = (targetPosition - position).normalized;
                position += moveDir * movingUnit.Speed * Time.deltaTime;
                transform.SetPositionAndRotation(position, Quaternion.identity);
                movingUnit.SavePosition(position);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) 
                    StopMoving();
            }
        }
        
        private void StopMoving() => pathVectorList = null;

        private Vector3 GetPosition() => transform.position;

        public void SetPathToDestination(RaycastHit hit)
        {
            SetTargetPosition(hit.point);
            AddHitPointAsFinalDestination(hit);
        }

        public void SetPathToDestination(Vector3 pos)
        {
            SetTargetPosition(pos);
            int elementsCount = pathVectorList.Count;
            pathVectorList[elementsCount - 1] = pos;
        }

        public PathfindingNode GetNode(Vector3 targetPos)
        {
            return PathfindingManager.Instance.GetNode(targetPos);
        }
        
        private void SetTargetPosition(Vector3 targetPosition)
        {
            currentPathIndex = 0;
            pathVectorList = PathfindingManager.Instance.FindPath(GetPosition(), targetPosition);
            
            if (pathVectorList != null && pathVectorList.Count > 1) 
                pathVectorList.RemoveAt(0);
        }

        private void AddHitPointAsFinalDestination(RaycastHit hit)
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (pathVectorList == null || hitLayer != terrainLayerMask) return;
            int elementsCount = pathVectorList.Count;
            pathVectorList[elementsCount - 1] = hit.point;
        }

        public void MarkAsLeader(IMovingUnits movingUnits) => leaderCrown.SetActive(movingUnit == movingUnits);
        public bool IsLeader => movingUnit.IsLeader;
    }
}