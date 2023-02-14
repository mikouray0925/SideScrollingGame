using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSight : Sight
{
    [SerializeField] float frontSightDistance;

    public override bool CanSeeTarget() {
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);
        return CanSeeTarget(endPos);
    }

    public override bool CanSeeTarget(out Transform target, out float distance) {
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);
        return CanSeeTarget(endPos, out target, out distance);
    }

    void OnDrawGizmosSelected() {
        if (!drawGizmosDebug) return;
        Vector2 endPos = eyePoint.position;
        endPos.x += frontSightDistance * Mathf.Sign(transform.localScale.x);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(eyePoint.position, endPos);
    }
}
