using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] public Transform followingTarget;
    [SerializeField] public Vector2 offsetFromTarget;
    public enum WayOfFollowing {
        Immediate,
        SmoothDamp
    }
    [SerializeField] public WayOfFollowing wayOfFollowing;
    [SerializeField] public float smoothTime;
    [SerializeField] public CombinedLock positionLock;
    Vector3 currentDampingVelocity;

    [Header ("Borders")]
    [SerializeField] float minX;
    [SerializeField] float minY;
    [SerializeField] float maxX;
    [SerializeField] float maxY;

    private void LateUpdate() {
        if (followingTarget && !positionLock.IsLocked) {
            Vector3 targetPos = Vec2Util.ToVec2(followingTarget.position) + offsetFromTarget;
            targetPos = Vec2Util.ToVec3(targetPos, transform.position.z);

            switch (wayOfFollowing) {
            case WayOfFollowing.Immediate:
                transform.position = targetPos;
                break;
            case WayOfFollowing.SmoothDamp:
                transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentDampingVelocity, smoothTime);
                break;
            }
        }

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}
