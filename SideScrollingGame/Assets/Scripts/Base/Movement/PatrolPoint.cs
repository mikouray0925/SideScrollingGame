using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//|=========================================================
//| Make the ojects like a linked list. The path formed by 
//| these points is the patrolling path.
//| 
//| 
//|=========================================================
public class PatrolPoint : MonoBehaviour
{
    [SerializeField] public PatrolPoint nextPoint;

    public Vector2 position {
        get {
            return transform.position;
        }
        set {
            transform.position = value;
        }
    }

    public float HowFar(Vector2 pos) {
        return (Vec2Util.ToVec2(transform.position) - pos).magnitude;
    }
}
