using System.Collections;
using Managers;
using UnityEngine;
using Utility;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsController : MonoBehaviour
    {
        [SerializeField] private MovingUnitsMono movingUnitsPrefab;
        [SerializeField] private Transform movingUnitsContent;

        IEnumerator Start()
        {
            WaitUntil waitUntil = new WaitUntil(() => MovingUnitsManager.Instance.IsReady);
            yield return waitUntil;
            
            SpawnUnitsMono();
        }

        private void SpawnUnitsMono()
        {
            movingUnitsContent.DestroyImmediateAllChildren();
            foreach (MovingUnit moveUnit in MovingUnitsManager.Instance.MovingList)
            {
                MovingUnitsMono movingUnitMono = Instantiate(movingUnitsPrefab, movingUnitsContent);
                movingUnitMono.Init(moveUnit);
            }
        }
    }
}
