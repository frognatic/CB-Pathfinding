using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using UI.Windows.Base;
using UnityEngine;

public class WindowsHolder : MonoBehaviour
{
    [SerializeField] private List<Window> windows;

    private void Start()
    {
        AddWindowsToManagerList();
    }

    public void AddWindowsToManagerList()
    {
        foreach (var window in windows)
        {
            WindowManager.Instance.AddWindow(window);
        }
    }
    
    [ContextMenu("Refill list")]
    public void RefillList() => windows = GetComponentsInChildren<Window>().ToList();
}
