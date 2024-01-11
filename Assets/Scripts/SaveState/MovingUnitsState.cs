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
    }

    public List<UnitData> movingUnitsData = new();
}
