using Gameplay.MovingUnits;
using Managers;
using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MovingUnits
{
    public class SetAsLeaderDisplay : MonoBehaviour
    {
        [SerializeField] private Image unitIcon;
        [SerializeField] private Image selectedImage;
        [SerializeField] private TextMeshProUGUI unitIdText;

        private MainWindow mainWindow;
        private MovingUnit movingUnit;

        public void Init(MainWindow mainWindow, MovingUnit movingUnit)
        {
            this.mainWindow = mainWindow;
            this.movingUnit = movingUnit;

            SetUnitInfo(movingUnit);
            MarkAsDeselected();
        }

        private void SetUnitInfo(IMovingUnitStats movingUnitStats)
        {
            unitIcon.color = movingUnitStats.Color;
            unitIdText.text = movingUnitStats.Id.ToString();
        }
        
        public void SetAsLeader()
        {
            if (IsUnitLeader(movingUnit))
                return;
            
            MovingUnitsManager.Instance.SetUnitAsLeader(movingUnit);
            MarkAsSelected();
        }

        private bool IsUnitLeader(IMovingUnits movingUnits) => movingUnits.IsLeader;

        private void MarkAsSelected()
        {
            mainWindow.DeselectAll();
            selectedImage.gameObject.SetActive(true);
        }

        public void MarkAsDeselected() => selectedImage.gameObject.SetActive(false);
    }
}
