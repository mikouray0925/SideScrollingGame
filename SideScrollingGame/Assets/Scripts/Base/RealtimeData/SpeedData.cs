using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedData : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] private float speed;

    [Header ("Value Adjuster")]
    [SerializeField] private CombinedMultiplier multiplier;

    public float Speed {
        get {
            if (multiplier) {
                return speed * multiplier.Multiplier;
            } else {
                return speed;
            }
        }
        private set {}
    }
}
