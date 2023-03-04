using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealth : Health
{
    [Header ("Color")]
    [SerializeField] Color hurtColor;
    
    Animator anim;
    Movement movement;
    
    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    private void Update() {
    }

    protected override void OnHealthIncrease() {

    }

    protected override void OnHealthDecrease() {
        MakeSpriteFlash(hurtColor, 0.1f);
    }

    protected override void OnReborn() {

    }

    protected override void OnLifeNumBecomeZero() {

    }

    protected override void OnTakingDamage(float damageVal, Vector2 damageDir, out float finalDamageVal) {
        anim.SetTrigger("takeHit");
        movement.Brake();
        movement.LockMovementForSeconds(0.2f);
        finalDamageVal = damageVal;
    }
}
