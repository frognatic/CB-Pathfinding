using System.Collections.Generic;
using Managers;
using Pathfinding;
using UnityEngine;

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
        private LayerMask unitLayerMask;
        
        private Vector3 targetPosition;

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
            unitLayerMask = LayerMask.NameToLayer("Units");
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
            if (IsPathVectorEmpty()) 
                return;

            SetTargetPosition();
            
            if (CanMove())
                Move();
            else
                TryNextStep();
        }

        private void SetTargetPosition()
        {
            targetPosition = pathVectorList[currentPathIndex];
            targetPosition.y = unitHeight;
        }

        private bool CanMove() => Vector3.Distance(transform.position, targetPosition) > GridRadius;

        private void Move()
        {
            Vector3 position = transform.position;
            Vector3 moveDir = (targetPosition - position).normalized;
            position += moveDir * movingUnit.Speed * Time.deltaTime;
            transform.SetPositionAndRotation(position, Quaternion.identity);
            movingUnit.SavePosition(position);
        }

        private void TryNextStep()
        {
            currentPathIndex++;
            if (currentPathIndex >= pathVectorList.Count) 
                StopMoving();
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
            if (IsPathVectorEmpty()) 
                return;
            
            int elementsCount = pathVectorList.Count;
            pathVectorList[elementsCount - 1] = pos;
        }

        public PathfindingNode GetNode(Vector3 targetPos) => PathfindingManager.Instance.GetNode(targetPos);

        private void SetTargetPosition(Vector3 pos)
        {
            currentPathIndex = 0;
            pathVectorList = PathfindingManager.Instance.FindPath(GetPosition(), pos);

            if (IsPathVectorEmpty()) return;
            pathVectorList.RemoveAt(0);
        }

        private void AddHitPointAsFinalDestination(RaycastHit hit)
        {
            int hitLayer = hit.collider.gameObject.layer;
            if (IsPathVectorEmpty() || hitLayer != terrainLayerMask) return;
            int elementsCount = pathVectorList.Count;
            pathVectorList[elementsCount - 1] = hit.point;
        }

        private bool IsPathVectorEmpty() => pathVectorList == null || pathVectorList.Count <= 0;

        public void MarkAsLeader(IMovingUnits movingUnits) => leaderCrown.SetActive(movingUnit == movingUnits);
        public bool IsLeader => movingUnit.IsLeader;
    }
}