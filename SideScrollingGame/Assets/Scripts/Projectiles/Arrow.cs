using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] float startFallingSpeed;
    [SerializeField] float lifespan;
    float remainingLifespan;
    bool isFalling;
    
    private void Awake() {
        GrabPhysicsComponents();
    }

    private void Update() {
        if (remainingLifespan > 0) {
            remainingLifespan -= Time.deltaTime;
        } else {
            Deactivate();
        }
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

    public override void Launch() {
        isFalling = false;
        rb.gravityScale = 0;
        remainingLifespan = lifespan;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            damageable.health.TakeDamage(damage, rb.velocity.normalized);
        }
        Deactivate();
    }
}
