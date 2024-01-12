using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.MovingUnits;
using Initial;
using Managers.Singleton;
using UnityEngine;
using Utility;

namespace Managers
{
    public class MovingUnitsManager : MonoSingleton<MovingUnitsManager>
    {
        public bool IsReady { get; private set; }
        
        private const int UnitsToStartAmount = 3;
        private MovingUnitsState movingUnitsState;

        public List<MovingUnit> MovingList { get; } = new();

        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Managers, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Managers);
            SaveManager.Instance.SaveState.movingUnitsState =
                movingUnitsState = SaveManager.Instance.SaveState.movingUnitsState ?? new();

            CreateUnits();

            IsReady = true;
        }

        private void CreateUnits()
        {
            MovingList.Clear();
            List<MovingUnitsState.UnitData> unitDataList = new();
            List<UnitDetails> randedUnitDetails = AddressableManager.Instance.GetUnitDetailsList()
                .GetRandomListElements(UnitsToStartAmount);
            
            for (int i = 0; i < UnitsToStartAmount; i++)
            {
                
                float randomSpeed = Random.Range(20f, 30f);
                MovingUnitsState.UnitData unitData = new(randedUnitDetails[i].id, randomSpeed, randedUnitDetails[i].spawnPosition);
                MovingUnit movingUnit = new(unitData, randedUnitDetails[i]);
                
                MovingList.Add(movingUnit);
                unitDataList.Add(unitData);
            }

            SaveManager.Instance.SaveState.movingUnitsState.movingUnitsData = unitDataList;
        }

        private void CreateSingleUnit()
        {
        }

        public void LoadUnits() => SaveManager.Instance.Load();

        public void SetUnitAsLeader()
        {
        }
    }
}