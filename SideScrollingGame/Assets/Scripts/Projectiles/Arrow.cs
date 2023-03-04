using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [SerializeField] float startFallingSpeed;
    [SerializeField] GameObject impactPointPrefab;
    [SerializeField] GameObject arrowHitPrefab;
    
    bool isFalling;
    
    private void Awake() {
        GrabPhysicsComponents();
    }

    private void Update() {
        UpdateLifespan();
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
        ResetLifespan();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            damageable.health.TakeDamage(damage, rb.velocity.normalized);
            if (GameManager.impactEffectHolder && damageable.impactPointHolder) {
                GameObject impactPoint = Instantiate(impactPointPrefab, transform.position, transform.rotation, damageable.impactPointHolder);
                GameObject arrowHit = Instantiate(arrowHitPrefab, transform.position, transform.rotation, GameManager.impactEffectHolder);
                if (transform.localScale.x < 0) {
                    arrowHit.transform.localScale = new Vector3(
                        -arrowHit.transform.localScale.x, 
                         arrowHit.transform.localScale.y, 
                         arrowHit.transform.localScale.z
                    );
                }
                arrowHit.GetComponent<ImpactFollowTrasnform>().Follow(impactPoint.transform);
            }
        }
        Deactivate();
    }
}
