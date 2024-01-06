using Managers.Singleton;

namespace Managers
{
    public class InitManager : MonoSingleton<InitManager>
    {
        public void Init()
        {
            AddressablesManager.Instance.Init();
            SaveManager.Instance.Init();
            WindowManager.Instance.Init();
            PathfindingManager.Instance.Init();
            MovingUnitsManager.Instance.Init();
        }
    }
}
