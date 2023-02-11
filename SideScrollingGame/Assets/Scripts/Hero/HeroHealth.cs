using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealth : Health
{
    [Header ("Color")]
    [SerializeField] Color hurtColor;
    
    Animator anim;
    
    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.K)) {
            TakeDamage(1f, Vector2.right);
        }
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
        finalDamageVal = damageVal;
    }
}
