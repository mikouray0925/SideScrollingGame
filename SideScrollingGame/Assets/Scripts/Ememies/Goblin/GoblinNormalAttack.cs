using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNormalAttack : Attack
{
    [Header ("Overlap")]
    [SerializeField] Overlap overlap;
    
    Movement movement;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        FinishAttackIfAnimNotPlaying();
    }

    public override bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD && movement.isGrounded;
    }

    public void UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.movementLock.AddLock("normalAttack", 0.4f);
            movement.Brake();
        } 
    }

    private void NormalAttackAnimStartEvent() {
        AttackAnimStart();
    }

    private void ApplyNormalAttackDamage() {
        ApplyDamage(overlap, new Damage(this, damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right));
    }

    protected override void OnAttackFinish() {
        movement.movementLock.RemoveLock("normalAttack");
        attackCD.StartCooldownCoroutine();
    }
}
