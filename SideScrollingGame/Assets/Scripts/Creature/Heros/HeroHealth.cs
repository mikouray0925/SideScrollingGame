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

    protected void OnHealthDecrease(float deltaHealth) {
        MakeSpriteFlash(hurtColor, 0.1f);
    }

    protected override void ProcessDamage(Damage damageInfo, out float finalDamageVal) {
        anim.SetTrigger("takeHit");
        movement.Brake();
        movement.movementLock.AddLock("takeHit", 0.2f);
        base.ProcessDamage(damageInfo, out finalDamageVal);
    }
}
