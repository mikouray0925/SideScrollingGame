using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : Health
{   
    Animator anim;
    
    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
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
    }

    protected override void OnTakingDamage(float damageVal, Vector2 damageDir, out float finalDamageVal) {
        anim.SetTrigger("takeHit");
        finalDamageVal = damageVal;
    }
}
