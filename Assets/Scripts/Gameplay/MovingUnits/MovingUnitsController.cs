using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Utility;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsController : MonoBehaviour
    {
        [SerializeField] private MovingUnitsMono movingUnitsPrefab;
        [SerializeField] private Transform movingUnitsContent;

        private List<MovingUnitsMono> movingUnitsMonoList = new();

        private void OnEnable()
        {
            MovingUnitsManager.OnChangeLeader += OnChangeLeaderResponse;
        }

        private void OnDisable()
        {
            MovingUnitsManager.OnChangeLeader += OnChangeLeaderResponse;
        }

        private IEnumerator Start()
        {
            WaitUntil waitUntil = new WaitUntil(() => MovingUnitsManager.Instance.IsReady);
            yield return waitUntil;

            SpawnUnitsMono();
        }

        private void SpawnUnitsMono()
        {
            Clear();
            foreach (MovingUnit moveUnit in MovingUnitsManager.Instance.MovingList)
            {
                MovingUnitsMono movingUnitMono = Instantiate(movingUnitsPrefab, movingUnitsContent);
                movingUnitMono.Init(moveUnit);
                movingUnitsMonoList.Add(movingUnitMono);
            }
        }

        private void Clear()
        {
            movingUnitsMonoList.Clear();
            movingUnitsContent.DestroyImmediateAllChildren();
        }

        private void OnChangeLeaderResponse(IMovingUnits movingUnit) =>
            movingUnitsMonoList.ForEach(x => x.MarkAsLeader(movingUnit));
    }
}