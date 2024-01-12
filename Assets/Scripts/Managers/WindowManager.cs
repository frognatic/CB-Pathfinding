using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers.Singleton;
using UI.Windows.Base;
using UnityEngine;

namespace Managers
{
    public class WindowManager : MonoSingleton<WindowManager>
    {
        [SerializeField] private List<Window> windowList = new();
        
        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => windowList.Count > 0, cancellationToken: Instance.destroyCancellationToken).ToAsyncLazy();
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
