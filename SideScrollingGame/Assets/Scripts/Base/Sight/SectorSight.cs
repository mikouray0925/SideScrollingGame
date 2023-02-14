using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorSight : Sight
{
    [SerializeField] Transform controlPoint;
    [Range (0f, 180f)]
    [SerializeField] float angle;
    

    public override bool CanSeeTarget() {
        return CanSeeTarget(out Transform target, out float distance);
    }

    public override bool CanSeeTarget(out Transform target, out float distance) {
        Vector2 toControlPoint = controlPoint.position - eyePoint.position;
        float radius = toControlPoint.magnitude;
        Collider2D[] collidersInCircle = Physics2D.OverlapCircleAll(eyePoint.position, radius, targetLayers);
        foreach (Collider2D collider in collidersInCircle) {
            if (collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
                Vector2 toDamageable = damageable.Center - Vec2Util.ToVec2(eyePoint.position);
                if (Vector2.Angle(toControlPoint, toDamageable) < (angle / 2f)) {
                    if (CanSeeTarget(damageable.Center, out target, out distance)) return true;
                }
            }
        }
        target = null;
        distance = Mathf.Infinity;
        return false;
    }

    void OnDrawGizmos() {
        if (!drawGizmosDebug) return;
        Gizmos.color = Color.white;

        Vector2 toControlPoint = controlPoint.position - eyePoint.position;
        float radius = toControlPoint.magnitude;
        //Gizmos.DrawWireSphere(eyePoint.position, radius);
        float halfAngle = (angle / 2f) * Mathf.Deg2Rad;
        Vector3  plusSideVec = Vec2Util.Rotate(toControlPoint, +halfAngle);
        Vector3 minusSideVec = Vec2Util.Rotate(toControlPoint, -halfAngle);
        Gizmos.DrawLine(eyePoint.position, eyePoint.position +  plusSideVec);
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + minusSideVec);
    }
}
