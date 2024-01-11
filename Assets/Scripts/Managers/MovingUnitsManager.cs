using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.MovingUnits;
using Initial;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class MovingUnitsManager : MonoSingleton<MovingUnitsManager>
    {
        private const int UnitsToStartAmount = 3;
        private MovingUnitsState movingUnitsState;

        public List<MovingUnits> MovingList { get; }

        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Managers, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Managers);
            SaveManager.Instance.SaveState.movingUnitsState =
                movingUnitsState = SaveManager.Instance.SaveState.movingUnitsState ?? new();
        }

        private void CreateUnits()
        {
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