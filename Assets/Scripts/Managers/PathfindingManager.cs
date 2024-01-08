using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;

namespace Managers
{
    public class PathfindingManager : MonoSingleton<PathfindingManager>
    {
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Managers, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Managers);
            // create grid etc.
        }
    }
}
