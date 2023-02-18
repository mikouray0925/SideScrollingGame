using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour
{
    [Header ("Debug")]
    [SerializeField] protected bool drawGizmosDebug;
    [SerializeField] protected Color sightColor;

    [Header ("Filter")]
    [SerializeField] protected LayerMask workingLayers;
    [SerializeField] protected LayerMask targetLayers;
    [SerializeField] protected string targetTag;

    [Header ("Data")]
    [SerializeField] protected Transform eyePoint;
    
    public virtual bool CanSeeTarget() {
        return false;
    }

    public virtual bool CanSeeTarget(out Transform target, out float distance) {
        target = null;
        distance = Mathf.Infinity;
        return false;
    }

    //|=========================================================
    //| Draw a line from "eyePoint" to "endPos", during the process: 
    //| If hit the collider with "targetTag", return true.
    //| If hit the collider with different tag or hit nothing,
    //| return false.
    //|=========================================================
    protected bool LinecastHitTarget(Vector2 endPos) {
        return LinecastHitTarget(endPos, out Transform target, out float distance);
    }

    //|=========================================================
    //| Draw a line from "eyePoint" to "endPos", during the process: 
    //| If hit the collider with "targetTag", return true.
    //| If hit the collider with different tag or hit nothing,
    //| return false.
    //|=========================================================
    protected bool LinecastHitTarget(Vector2 endPos, out Transform target, out float distance) {
        RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, endPos, workingLayers);
        if (hit.collider != null) {
            if (drawGizmosDebug) {
                Debug.DrawLine(eyePoint.position, hit.point, sightColor);
            }
            if (hit.collider.tag == targetTag) {
                target = hit.collider.transform;
                distance = (hit.point - Vec2Util.ToVec2(eyePoint.position)).magnitude;
                return true;
            }
        }
        target = null;
        distance = Mathf.Infinity;
        return false;
    }
}
