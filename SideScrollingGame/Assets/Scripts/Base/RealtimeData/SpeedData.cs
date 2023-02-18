using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedData : DataWithMultiplier
{
    [Header ("Parameters")]
    [SerializeField] private float speed;

    public float Speed {
        get {
            return speed * Multiplier;
        }
        private set {}
    }
}
