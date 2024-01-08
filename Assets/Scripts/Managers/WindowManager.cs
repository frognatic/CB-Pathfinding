using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;

namespace Managers
{
    public class WindowManager : MonoSingleton<WindowManager>
    {
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Windows, Initialize().ToAsyncLazy());
        
        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Windows);
            // OpenDefaultWindow
        }
    }
}
