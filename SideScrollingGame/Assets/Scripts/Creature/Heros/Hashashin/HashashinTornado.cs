using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinTornado : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] float timeNeededToStop;
    [SerializeField] float force;
    
    Animator anim;
    Collider2D col;
    Rigidbody2D rb;

    float currentDampingVelocity;
    Damage damage;
    HashSet<Health> damagedHealth = new HashSet<Health>();
    
    private void Awake() {
        anim = GetComponent<Animator>();
        col  = GetComponent<Collider2D>();
        rb   = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        float newVelocity = Mathf.SmoothDamp(rb.velocity.x, 0, ref currentDampingVelocity, timeNeededToStop);
        rb.velocity = new Vector2(newVelocity, 0);
        UpdateDamageForce();
    }

    private void UpdateDamageForce() {
        if (damage != null && damage.forces.Count > 0) {
            damage.forces[0].force = rb.velocity * force;
        }
    }
    
    public void Activate(Vector2 pos, float towardSide, float startSpeed, Damage _damage) {
        transform.position = pos;
        if (Mathf.Sign(transform.localScale.x) != Mathf.Sign(towardSide)) {
            transform.localScale = new Vector3(
                -transform.localScale.x,
                 transform.localScale.y,
                 transform.localScale.z
            );
        }

        gameObject.SetActive(true);
        anim.SetTrigger("restart");

        col.enabled = true;
        rb.velocity = Mathf.Sign(transform.localScale.x) * startSpeed * Vector2.right;
        currentDampingVelocity = 0;

        damage = _damage;
        damagedHealth.Clear();
        UpdateDamageForce();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
            if (!damagedHealth.Contains(damageable.health)) {
                damageable.health.TakeDamage(damage);
                damagedHealth.Add(damageable.health);
            } else if (damageable.health.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) {
                if (damage != null && damage.forces.Count > 0) {
                    rb.AddForce(damage.forces[0].force, damage.forces[0].mode);
                }
            }
        }
    }

    public void Destroy() {
        anim.SetTrigger("destroy");
        rb.velocity = Vector2.zero;
        col.enabled = false;
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
    
}
