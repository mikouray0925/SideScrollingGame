using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSight : Sight
{
    [SerializeField] float frontSightDistance;
    [SerializeField] bool drawGizmosDebug;
    [SerializeField] Color sightColor;

    public override bool CanSeeTarget() {
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);

        RaycastHit2D hit = Physics2D.Linecast(eyePoint.position, endPos, workingLayers);
        if (hit.collider != null) {
            if (drawGizmosDebug) {
                Debug.DrawLine(eyePoint.position, hit.point, sightColor);
            }
            if (hit.collider.tag == targetTag) {
                return true;
            }
        }
        return false;
    }

    public override bool CanSeeTarget(out Transform target, out float distance) {
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);

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

    void OnDrawGizmosSelected() {
        if (!drawGizmosDebug) return;
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(eyePoint.position, endPos);
    }
}
