using UnityEngine;

namespace Gameplay.MovingUnits
{
    public interface IMovingUnitStats
    {
        public int Id { get; }
        public float Speed { get; }
        public Vector3 Position { get;}
        public Color Color { get; }
    }
}
