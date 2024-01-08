using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;

namespace Managers
{
    public class AddressablesManager : MonoSingleton<AddressablesManager>
    {
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Addressable, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Addressable);
            // Load Addressables
        }
    }
}
