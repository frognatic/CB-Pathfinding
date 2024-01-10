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

        public void Init(MovingUnits movingUnits)
        {
            this.movingUnits = movingUnits;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            HandleMovement();

            if (Input.GetMouseButtonDown(0))
            {
                SetTargetPosition(GetMouseWorldPosition());
            }
        }

        private void HandleMovement()
        {
            if (pathVectorList != null)
            {
                Vector3 targetPosition = pathVectorList[currentPathIndex];
                if (Vector3.Distance(transform.position, targetPosition) > 1f)
                {
                    Vector3 moveDir = (targetPosition - transform.position).normalized;

                    transform.position += moveDir * 40 * Time.deltaTime;
                }
                else
                {
                    currentPathIndex++;
                    if (currentPathIndex >= pathVectorList.Count)
                    {
                        StopMoving();
                    }
                }
            }
        }

        private void StopMoving()
        {
            pathVectorList = null;
        }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            currentPathIndex = 0;
            pathVectorList = PathfindingManager.Instance.FindPath(GetPosition(), targetPosition);
            
            //Debug.LogWarning($"Last: {}");

            if (pathVectorList != null && pathVectorList.Count > 1)
            {
                pathVectorList.RemoveAt(0);
            }
        }

        public Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            //vec.z = 0f;
            return vec;
        }

        public Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
        {
            Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
            return worldPosition;
        }
    }
}