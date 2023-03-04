using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : Health
{   
    Animator anim;
    Movement movement;
    GoblinBehavior behavior;
    
    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        behavior = GetComponent<GoblinBehavior>();
    }

    private void Update() {
    }

    protected override void OnHealthIncrease() {

    }

    protected override void OnHealthDecrease() {
    }

    protected override void OnReborn() {
    }

    protected override void OnLifeNumBecomeZero() {
        anim.SetTrigger("die");
        behavior.enabled = false;
        movement.horizInput = 0;
        movement.Brake();
        movement.enabled = false;
    }

    protected override void OnTakingDamage(float damageVal, Vector2 damageDir, out float finalDamageVal) {
        anim.SetTrigger("takeHit");
        if (Mathf.Sign(damageDir.x) == Mathf.Sign(transform.localScale.x)) movement.Flip();
        movement.Brake();
        movement.LockMovementForSeconds(0.2f);
        finalDamageVal = damageVal;
    }
}
