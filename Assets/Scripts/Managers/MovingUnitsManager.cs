using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.MovingUnits;
using Initial;
using Managers.Singleton;
using Utility;
using Random = UnityEngine.Random;

namespace Managers
{
    public class MovingUnitsManager : MonoSingleton<MovingUnitsManager>
    {
        public bool IsReady { get; private set; }
        
        public static event Action<IMovingUnits> OnChangeLeader;
        
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
                unitDataList.Add(CreateSingleUnit(randedUnitDetails[i]));
            }

            SaveManager.Instance.SaveState.movingUnitsState.movingUnitsData = unitDataList;
        }
        
        private MovingUnitsState.UnitData CreateSingleUnit(UnitDetails unitDetails)
        {
            float randomSpeed = GetRandomSpeed();
            MovingUnitsState.UnitData unitData = new(unitDetails.id, randomSpeed, unitDetails.spawnPosition);
            MovingUnit movingUnit = new(unitData, unitDetails);
                
            MovingList.Add(movingUnit);
            return unitData;
        }
        
        private float GetRandomSpeed() => Random.Range(20f, 30f);
        
        public void LoadUnits() => SaveManager.Instance.Load();

        public void SetUnitAsLeader(IMovingUnits movingUnits)
        {
            MarkAsDeselected();
            movingUnits.MarkAsLeader();
            
            OnChangeLeader?.Invoke(movingUnits);
        }

        private void MarkAsDeselected()
        {
            MovingList.ForEach(x => x.Unmark());
        }
    }
}