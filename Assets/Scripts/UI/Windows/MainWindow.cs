using System.Collections.Generic;
using Gameplay.MovingUnits;
using Managers;
using UI.MovingUnits;
using UI.Windows.Base;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI.Windows
{
    public class MainWindow : Window
    {
        [Header("Set as leader displays")]
        [SerializeField] private SetAsLeaderDisplay setLeaderPrefab;
        [SerializeField] private Transform setLeaderContent;

        [Header("Save / load buttons")] 
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;

        private readonly List<SetAsLeaderDisplay> setAsLeaderDisplayList = new();
        
        public override void OnOpen()
        {
            base.OnOpen();
            InitLeaderDisplays();
            LoadButtonInteractionStatus();
            
            SaveManager.SavingAction += SaveManagerOnSavingAction;
        }

        public override void OnClose()
        {
            base.OnClose();
            
            SaveManager.SavingAction -= SaveManagerOnSavingAction;
        }

        private void LoadButtonInteractionStatus() => loadButton.interactable = SaveManager.Instance.HasSave();

        private void SaveManagerOnSavingAction(bool isSaving)
        {
            saveButton.interactable = !isSaving;
            loadButton.interactable = !isSaving && SaveManager.Instance.HasSave();
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
