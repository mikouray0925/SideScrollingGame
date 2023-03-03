using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSmallArrow : Projectile
{
    public RangerRootWave rootWave;
    
    private void Awake() {
        GrabPhysicsComponents();
    }

    private void Update() {
        UpdateLifespan();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        List<ContactPoint2D> contacts = new List<ContactPoint2D>();
        int contactNum = other.GetContacts(contacts);
        if (rootWave && !rootWave.IsActive && contactNum > 0 &&
            GameManager.InLayerMask(other.collider.gameObject, GameManager.groundLayers)) {
            if (Vector2.Dot(contacts[0].normal, Vector2.up) > 0.9f) {
                Vector3 pos = rootWave.transform.position;
                pos.y = contacts[0].point.y;
                rootWave.transform.position = pos;
                rootWave.Activate();
            }
        }

        if (other.collider.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            damageable.health.TakeDamage(damage, Vector2.down);
        }
        Deactivate();
    }
}
