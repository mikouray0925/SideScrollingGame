using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSmallArrow : Projectile
{
    public RangerRootWave rootWave;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip hitFleshSFX;

    public ObjPool<FallingSmallArrow> inPool;
    
    private void Awake() {
        GrabPhysicsComponents();
    }

    private void Update() {
        UpdateLifespan();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (LayerUtil.Judge(other.collider).IsInMask(GlobalSettings.creatureLayers)) {
            AudioSource.PlayClipAtPoint(hitFleshSFX, transform.position, AudioManager.effectVolume);
        } else {
            AudioSource.PlayClipAtPoint(hitSFX, transform.position, AudioManager.effectVolume);
        }

        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        int contactNum = other.GetContacts(contacts);
        if (rootWave && !rootWave.IsActive && contactNum > 0 &&
            LayerUtil.Judge(other.collider).IsInMask(GlobalSettings.groundLayers)) {
            if (Vector2.Dot(contacts[0].normal, Vector2.up) > 0.9f) {
                Vector3 pos = rootWave.transform.position;
                pos.y = contacts[0].point.y;
                rootWave.transform.position = pos;
                rootWave.Activate();
            }
        }

        if (other.collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            if (damage != null) {
                damage.mainDirection = Vector2.down;
                damageable.health.TakeDamage(damage);
            }
        }
        Deactivate();
    }

    void OnDeactivateThisProjectile() {
        if (inPool != null) inPool.Recycle(this);
    }
}
