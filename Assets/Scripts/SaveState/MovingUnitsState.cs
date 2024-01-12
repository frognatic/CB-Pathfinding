using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class MovingUnitsState
{
    [Serializable]
    public class UnitData
    {
        public int id;
        public float speed;
        public Vector3 position;

        private const float MinSpeed = 20f;
        private const float MaxSpeed = 30f;

        public UnitData(int id, float speed, Vector3 position)
        {
            this.id = id;
            this.speed = speed;
            this.position = position;
        }
        
        public UnitData(int id, Vector3 position)
        {
            this.id = id;
            speed = GetRandomSpeed();
            this.position = position;
        }
        
        private float GetRandomSpeed() => Random.Range(MinSpeed, MaxSpeed);
    }

    public List<UnitData> movingUnitsData = new();
}
