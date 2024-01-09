using UnityEngine;

namespace Gameplay.MovingUnits
{
    public class MovingUnits: IMovingUnitStats, IMovingUnits
    {
        private Vector3 defaultPosition = Vector3.zero;
        private MovingUnitState.UnitData saveUnitData;
        
        public int Id { get; set; }
        public float Speed { get; set; }
        public Vector3 Position { get; set; }
        
        public void MarkAsLeader() {}
        public void Unmark() {}
    }
}
