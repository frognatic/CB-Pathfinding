using UnityEngine;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsMono : MonoBehaviour
    {
        private MovingUnits movingUnits;

        public void Init(MovingUnits movingUnits)
        {
            this.movingUnits = movingUnits;
        }
    }
}
