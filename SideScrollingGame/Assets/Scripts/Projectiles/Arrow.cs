using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] float startFallingSpeed;
    bool isFalling;
    
    private void Awake() {
        GrabPhysicsComponents();
    }

    public override void Launch() {
        isFalling = false;
        rb.gravityScale = 0;
    }

    private void FixedUpdate() {
        RotationFollowVelocity();
        if (!isFalling && rb.velocity.magnitude < startFallingSpeed) {
            isFalling = true;
        }
        if (isFalling) {
            rb.gravityScale = 1;
        }
        else {
            rb.gravityScale = 0;
            ApplyAirResistance();
        }
    }
}
