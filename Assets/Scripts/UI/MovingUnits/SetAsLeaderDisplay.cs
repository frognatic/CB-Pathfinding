using Gameplay.MovingUnits;
using Managers;
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

        private const string UnitSprite = "unit_";

        public void Init(IMovingUnitStats movingUnitStats)
        {
            string spriteId = $"{UnitSprite}{movingUnitStats.Id}";
            unitIcon.sprite = AddressableManager.Instance.GetSprite(spriteId);
            unitIdText.text = movingUnitStats.Id.ToString();

            MarkAsDeselected();
        }
        
        public void SetAsLeader()
        {
            MarkAsSelected();
        }

        private void MarkAsSelected() => selectedImage.gameObject.SetActive(true);
        private void MarkAsDeselected() => selectedImage.gameObject.SetActive(false);
    }
}
