using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsMono : MonoBehaviour
    {
        private MovingUnits movingUnits;

        private int currentPathIndex;
        private List<Vector3> pathVectorList;

        private float unitHeight;
        private LayerMask terrainLayerMask;
        private Camera mainCamera;

        private float GridRadius => PathfindingManager.Instance.Grid.NodeRadius;

        public void Init(MovingUnits movingUnits) => this.movingUnits = movingUnits;

        private void Awake()
        {
            unitHeight = transform.position.y;
            terrainLayerMask = LayerMask.NameToLayer("Terrain");
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!CanUnitMove())
                return;

            HandleMovement();
            HandleInput();
        }

        private bool CanUnitMove() => PathfindingManager.Instance.Grid != null && 
                                      !EventSystem.current.IsPointerOverGameObject();

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
                position += moveDir * 40 * Time.deltaTime;
                transform.SetPositionAndRotation(position, Quaternion.identity);
            }
            else
            {
                currentPathIndex++;
                if (currentPathIndex >= pathVectorList.Count) 
                    StopMoving();
            }
        }

        private void HandleInput()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 100)) 
                return;
                
            SetTargetPosition(hit.point);
            AddHitPointAsFinalDestination(hit);
        }
        
        private void StopMoving() => pathVectorList = null;

        private Vector3 GetPosition() => transform.position;

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
    }
}