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

    private void OnLifeNumBecomeZero() {
        anim.SetTrigger("die");
        behavior.enabled = false;
        movement.horizInput = 0;
        movement.Brake();
        movement.enabled = false;
    }

    protected override void ProcessDamage(Damage damageInfo, out float finalDamageVal) {
        ProcessDamageDefault(damageInfo);
        anim.SetTrigger("takeHit");
        if (Mathf.Sign(damageInfo.mainDirection.x) == Mathf.Sign(transform.localScale.x)) movement.Flip();
        movement.Brake();
        movement.LockMovementForSeconds(0.2f);
        finalDamageVal = damageInfo.damage;
    }
}
