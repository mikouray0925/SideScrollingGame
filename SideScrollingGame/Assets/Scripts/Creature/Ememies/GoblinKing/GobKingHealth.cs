using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobKingHealth : Health
{
    [SerializeField] Transform[] impactPointHolders;
    
    Collider2D col;
    Rigidbody2D rb;
    Animator anim;
    Movement movement;
    GobKingBehavior behavior;
    
    private void Awake() {
        col  = GetComponent<Collider2D>();
        rb   = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        behavior = GetComponent<GobKingBehavior>();
    }

    private void OnLifeNumBecomeZero() {
        anim.SetTrigger("die");
        behavior.enabled = false;
        movement.horizInput = 0;
        movement.Brake();
        movement.enabled = false;

        rb.gravityScale = 0;
        col.enabled = false;

        foreach (Transform holder in impactPointHolders) {
            foreach (Transform t in holder) {
                Destroy(t.gameObject);
            }   
        }
    }

    protected override void ProcessDamage(Damage damageInfo, out float finalDamageVal) {
        base.ProcessDamage(damageInfo, out finalDamageVal);
        anim.SetTrigger("takeHit");
        if (Mathf.Sign(damageInfo.mainDirection.x) == Mathf.Sign(transform.localScale.x)) {
            Invoke(nameof(TurnBack), 0.25f);
        }
        movement.Brake();
        movement.movementLock.AddLock("takeHit", 0.2f);
    }

    private void TurnBack() {
        movement.Flip();
    }
}