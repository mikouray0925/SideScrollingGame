using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] public Damage damage;
    [SerializeField] public float airResistanceCoefficient;
    [SerializeField] public float lifespan;
    float remainingLifespan;

    public Collider2D col {get; protected set;}
    public Rigidbody2D rb {get; protected set;}

    public bool GrabPhysicsComponents() {
        col = GetComponent<Collider2D>();
        rb  = GetComponent<Rigidbody2D>();

        return col && rb;
    }

    public bool IsActive {
        get {
            return gameObject.activeSelf;
        }
        set {
            if ( value && !gameObject.activeSelf) Activate();
            if (!value &&  gameObject.activeSelf) Deactivate();
        }
    }

    public virtual void Activate() {
        gameObject.SetActive(true);
    }

    public virtual void Launch() {
        ResetLifespan();
    }

    public virtual void Deactivate() {
        gameObject.SetActive(false);
    }

    protected virtual void RotationFollowVelocity() {
        if (transform.localScale.x > 0) {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -Vector2.Angle(Vector2.right, rb.velocity.normalized));
        }
        else {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, Vector2.Angle(Vector2.left, rb.velocity.normalized));
        }
        
    }

    protected virtual void ApplyAirResistance() {
        if (!rb) return;
        float speed = rb.velocity.magnitude;
        float speedSubtrahend = speed * speed * airResistanceCoefficient;
        rb.velocity = rb.velocity.normalized * (speed - speedSubtrahend);
    }

    protected void ResetLifespan() {
        remainingLifespan = lifespan;
    }

    protected virtual void UpdateLifespan() {
        if (remainingLifespan > 0) {
            remainingLifespan -= Time.deltaTime;
        } else {
            Deactivate();
        }
    }
}
