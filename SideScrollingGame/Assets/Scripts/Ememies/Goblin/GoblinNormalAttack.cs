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
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishNormalAttack();
    }

    public override bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD && movement.isGrounded;
    }

    public bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(0.4f);
            movement.Brake();
        } 
        return isAttacking;
    }

    private void NormalAttackAnimStartEvent() {
        isAttacking = true;
    }

    private void ApplyNormalAttackDamage() {
        ApplyDamage(overlap, new Damage(this, damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right));
    }

    private void FinishNormalAttack() {
        movement.UnlockMovement();
        attackCD.StartCooldownCoroutine();
        isAttacking = false;
    }
}
