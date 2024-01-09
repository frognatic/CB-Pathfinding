using UnityEngine;

namespace Gameplay.MovingUnits
{
    public interface IMovingUnitStats
    {
        public int Id { get; set; }
        public float Speed { get; set; }
        public Vector3 Position { get; set; }
    }
}
