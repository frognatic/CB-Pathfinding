using Cysharp.Threading.Tasks;
using Managers.Singleton;

namespace Managers
{
    public class InitManager : MonoSingleton<InitManager>
    {
        public async UniTaskVoid Init()
        {
            await SaveSOManager.Instance.Initialize();
            await SaveManager.Instance.Initialize();
            await AddressableManager.Instance.Initialize();
            await PathfindingManager.Instance.Initialize();
            await MovingUnitsManager.Instance.Initialize();
            await WindowManager.Instance.Initialize();
        }
    }
}
