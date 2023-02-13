using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapBox : Overlap
{
    [SerializeField] Vector2 size;
    
    public override Collider2D[] GetOverlapColliders() {
        return Physics2D.OverlapBoxAll(transform.position, size, transform.eulerAngles.z, targetLayers);
    }

    public override void OnDrawGizmosShape() {
        Vector2 center = transform.position;
        Vector2 halfSize = size / 2f;
        float radAngle = transform.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 topLeft  = Vec2Util.Rotate(new Vector2(-halfSize.x, halfSize.y), radAngle);
        Vector2 topRight = Vec2Util.Rotate(new Vector2(+halfSize.x, halfSize.y), radAngle);
        Vector2 bottomLeft  = Vec2Util.Rotate(new Vector2(-halfSize.x, -halfSize.y), radAngle);
        Vector2 bottomRight = Vec2Util.Rotate(new Vector2(+halfSize.x, -halfSize.y), radAngle);
        Gizmos.DrawLine(center + topLeft, center + topRight);
        Gizmos.DrawLine(center + topRight, center + bottomRight);
        Gizmos.DrawLine(center + bottomRight, center + bottomLeft);
        Gizmos.DrawLine(center + bottomLeft, center + topLeft);
    }
}
