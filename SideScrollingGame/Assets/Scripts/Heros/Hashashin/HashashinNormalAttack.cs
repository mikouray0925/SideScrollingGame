using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinNormalAttack : HeroNormalAttack
{
    [Header ("Overlaps")]
    [SerializeField] Overlap overlap1;
    [SerializeField] Overlap overlap2;

    Movement movement;

    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    protected override void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishNormalAttack();
        base.Update();
    }

    public override bool AbleToAttack() {
        return 
            !isAttacking &&
            !attackCD.IsInCD && 
            movement.isGrounded && 
            !movement.isJumping && 
            !movement.isRolling;
    }

    protected override bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(0.5f);
            movement.Brake();
            isAttacking = true;
        } 
        return isAttacking;
    }

    private void ApplyNormalAttackDamage1() {
        ApplyDamage(overlap1, Mathf.Sign(transform.localScale.x) * Vector2.right);
    }

    private void ApplyNormalAttackDamage2() {
        ApplyDamage(overlap2, Mathf.Sign(transform.localScale.x) * Vector2.right);
    }

    protected override void FinishNormalAttack() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
