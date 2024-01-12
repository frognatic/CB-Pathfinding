using Gameplay.MovingUnits;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MovingUnits
{
    public class SetAsLeaderDisplay : MonoBehaviour
    {
        [SerializeField] private Image unitIcon;
        [SerializeField] private Image selectedImage;
        [SerializeField] private TextMeshProUGUI unitIdText;
        
        private UnitDetails unitDetails;

        public void Init(IMovingUnitStats movingUnitStats)
        {
            unitIcon.color = movingUnitStats.Color;
            unitIdText.text = movingUnitStats.Id.ToString();

            MarkAsDeselected();
        }
        
        public void SetAsLeader() => MarkAsSelected();

        private void MarkAsSelected() => selectedImage.gameObject.SetActive(true);
        private void MarkAsDeselected() => selectedImage.gameObject.SetActive(false);
    }
}
