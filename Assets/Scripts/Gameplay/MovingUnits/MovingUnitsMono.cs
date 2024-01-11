using System.Collections.Generic;
using System.Linq;
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
            if (PathfindingManager.Instance.Grid == null)
                return;
            
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            //HandleMovement();

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.LogWarning($"{hit.point}");
                    SetTargetPosition(hit.point);
                    SetPosition();
                }
            }
        }

        private void HandleMovement()
        {
            if (pathVectorList != null)
            {
                Vector3 targetPosition = pathVectorList[currentPathIndex];
                if (Vector3.Distance(transform.position, targetPosition) > 0.5f)
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

        private void SetPosition()
        {
            transform.position = pathVectorList.Last();
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
    }
}