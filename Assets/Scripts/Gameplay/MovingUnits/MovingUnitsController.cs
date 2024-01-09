using Managers;
using UnityEngine;
using Utility;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsController : MonoBehaviour
    {
        [SerializeField] private MovingUnitsMono movingUnitsPrefab;
        [SerializeField] private Transform movingUnitsContent;

        private void SpawnUnitsMono()
        {
            movingUnitsContent.DestroyImmediateAllChildren();
            foreach (MovingUnits moveUnit in MovingUnitsManager.Instance.MovingList)
            {
                MovingUnitsMono movingUnitMono = Instantiate(movingUnitsPrefab, movingUnitsContent);
                movingUnitMono.Init(moveUnit);
            }
        }
    }
}
