using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.MovingUnits;
using Initial;
using Managers.Singleton;

namespace Managers
{
    public class MovingUnitsManager : MonoSingleton<MovingUnitsManager>
    {
        private const int UnitsToStartAmount = 3;

        public List<MovingUnits> MovingList { get; }

        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Managers, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Managers);
            // spawn default players etc.
        }

        private void CreateUnits() { }
        
        private void CreateSingleUnit() {}

        public void LoadUnits() {}

        public void SetUnitAsLeader() {}
    }
}
