using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [SerializeField] protected Transform eyePoint;
    [SerializeField] protected LayerMask workingLayers;
    [SerializeField] protected string targetTag;
    
    public virtual bool CanSeeTarget() {
        return false;
    }

    public virtual bool CanSeeTarget(out Transform target, out float distance) {
        target = null;
        distance = Mathf.Infinity;
        return false;
    }
}
