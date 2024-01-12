using System.Collections.Generic;
using Gameplay.MovingUnits;
using Managers;
using UI.MovingUnits;
using UI.Windows.Base;
using UnityEngine;
using Utility;

namespace UI.Windows
{
    public class MainWindow : Window
    {
        [SerializeField] private SetAsLeaderDisplay setLeaderPrefab;
        [SerializeField] private Transform setLeaderContent;

        private readonly List<SetAsLeaderDisplay> setAsLeaderDisplayList = new();
        
        public override void OnOpen()
        {
            base.OnOpen();
            InitLeaderDisplays();
        }

        private void InitLeaderDisplays()
        {
            Clear();
            CreateDisplays();   
        }

        private void CreateDisplays()
        {
            foreach (MovingUnit movingUnit in MovingUnitsManager.Instance.MovingList)
            {
                SetAsLeaderDisplay setAsLeaderDisplay = Instantiate(setLeaderPrefab, setLeaderContent);
                setAsLeaderDisplay.Init(this, movingUnit);
                
                setAsLeaderDisplayList.Add(setAsLeaderDisplay);
            }
        }

        private void Clear()
        {
            setLeaderContent.DestroyImmediateAllChildren();
            setAsLeaderDisplayList.Clear();
        }

        public void DeselectAll() => setAsLeaderDisplayList.ForEach(x => x.MarkAsDeselected()); 

        public void Save() => SaveManager.Instance.TrySave();

        public void Load()
        {
            MovingUnitsManager.Instance.LoadMovingUnits();
            InitLeaderDisplays();
        }
    }
}
