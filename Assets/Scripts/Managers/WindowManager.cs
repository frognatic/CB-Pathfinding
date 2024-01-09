using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class WindowManager : MonoSingleton<WindowManager>
    {
        [SerializeField] private List<Window> windowList = new();
        
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Windows, Initialize().ToAsyncLazy());
        
        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Windows);
            await UniTask.WaitUntil(() => windowList.Count > 0, cancellationToken: Instance.destroyCancellationToken).ToAsyncLazy();
            Debug.LogWarning("TEST");
            OpenWindow(WindowType.MainWindow);
        }

        public void AddWindow(Window window)
        {
            if (windowList.Contains(window)) return;
            windowList.Add(window);
        }
        
        public void OpenWindow(WindowType windowType) => GetWindow(windowType).Open();

        public void CloseWindow(WindowType windowType) => GetWindow(windowType).Close();

        private Window GetWindow(WindowType windowType) => windowList.Find(x => x.WindowType == windowType);
    }
}
