using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    [Header ("Arrow")]
    [SerializeField] float startFallingSpeed;
    [SerializeField] GameObject impactPointPrefab;
    [SerializeField] GameObject arrowHitPrefab;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip hitFleshSFX;
    
    public ObjPool<Arrow> inPool;
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
        if (LayerUtil.Judge(other.collider).IsInMask(GlobalSettings.creatureLayers)) {
            AudioSource.PlayClipAtPoint(hitFleshSFX, transform.position, AudioManager.effectVolume);
        } else {
            AudioSource.PlayClipAtPoint(hitSFX, transform.position, AudioManager.effectVolume);
        }

        GameObject arrowHit = null;
        if (GameManager.impactEffectHolder) {
            arrowHit = Instantiate(arrowHitPrefab, transform.position, transform.rotation, GameManager.impactEffectHolder);
            if (transform.localScale.x < 0) {
                 arrowHit.transform.localScale = new Vector3(
                -arrowHit.transform.localScale.x, 
                 arrowHit.transform.localScale.y, 
                 arrowHit.transform.localScale.z
                );
            }
        }

        if (other.collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            if (arrowHit && damageable.impactPointHolder) {
                GameObject impactPoint = Instantiate(impactPointPrefab, transform.position, transform.rotation, damageable.impactPointHolder);
                arrowHit.GetComponent<ImpactEffectSystem>().Follow(impactPoint.transform);
            }

            if (damage != null) {
                // if (rb.velocity != Vector2.zero) damage.mainDirection = rb.velocity.normalized;
                damageable.health.TakeDamage(damage);
            }
        }
        Deactivate();
    }

    void OnDeactivateThisProjectile() {
        if (inPool != null) inPool.Recycle(this);
    }
}
