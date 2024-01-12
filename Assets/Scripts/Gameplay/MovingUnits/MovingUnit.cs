using UnityEngine;

namespace Gameplay.MovingUnits
{
    public class MovingUnit: IMovingUnitStats, IMovingUnits
    {
        private readonly MovingUnitsState.UnitData saveUnitData;
        private readonly UnitDetails unitDetails;

        public int Id => saveUnitData.id;
        public float Speed => saveUnitData.speed;
        public Vector3 Position => saveUnitData.position;
        public Color Color => unitDetails.color;

        public MovingUnit(MovingUnitsState.UnitData saveUnitData, UnitDetails unitDetails)
        {
            this.saveUnitData = saveUnitData;
            this.unitDetails = unitDetails;
        }
        
        public void MarkAsLeader() {}
        public void Unmark() {}
    }
}
