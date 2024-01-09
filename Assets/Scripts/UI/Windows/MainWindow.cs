using Managers;
using UI.MovingUnits;
using UI.Windows.Base;
using UnityEngine;

namespace UI.Windows
{
    public class MainWindow : Window
    {
        [SerializeField] private SetAsLeaderDisplay setLeaderPrefab;
        [SerializeField] private Transform setLeaderContent;
        
        public override void OnOpen()
        {
            base.OnOpen();
        }

        public override void OnClose()
        {
            base.OnClose();
        }

        public void Save() => SaveManager.Instance.Save();

        public void Load() => MovingUnitsManager.Instance.LoadUnits();
    }
}
