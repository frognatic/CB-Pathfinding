using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MovingUnitsState
{
    [Serializable]
    public class UnitData
    {
        public int id;
        public float speed;
        public Vector3 position;

        public UnitData(int id, float speed, Vector3 position)
        {
            this.id = id;
            this.speed = speed;
            this.position = position;
        }
    }

    public List<UnitData> movingUnitsData = new();
}
