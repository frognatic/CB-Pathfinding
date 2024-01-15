using System.Collections.Generic;
using System.Linq;
using Managers;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Windows.Base
{
    public class WindowsHolder : MonoBehaviour
    {
        [SerializeField] private List<Window> windows;

        private void Start() => AddWindowsToManagerList();

        private void AddWindowsToManagerList()
        {
            foreach (var window in windows) 
                WindowManager.Instance.AddWindow(window);
        }
    
        [Button("Refill List")]
        private void RefillList() => windows = GetComponentsInChildren<Window>().ToList();
    }
}
