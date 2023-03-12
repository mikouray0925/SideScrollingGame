using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinNormalAttack : HeroNormalAttack
{
    [Header ("Overlaps")]
    [SerializeField] Overlap overlap1;
    [SerializeField] Overlap overlap2;

    [Header ("Impact")]
    [SerializeField] GameObject impact1;
    [SerializeField] GameObject impact2;

    Movement movement;

    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    protected void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishNormalAttack();
    }

    public override bool AbleToAttack() {
        return 
            !isAttacking &&
            !attackCD.IsInCD && 
            movement.isGrounded && 
            !movement.isJumping && 
            !movement.isRolling;
    }

    public override bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(0.5f);
            movement.Brake();
        } 
        return isAttacking;
    }

    private void ApplyNormalAttackDamage1() {
        List<DamageablePart> damageableList = overlap1.GetOverlapDamageableParts();
        foreach (DamageablePart damageable in damageableList) {
            ImpactEffectSystem impact = GameManager.SpawnImpactEffect(impact1);
            impact.transform.position = damageable.Center;
            if (transform.localScale.x < 0) impact.Flip();
        }

        ApplyDamage(damageableList, new Damage(this, damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right));
    }

    private void ApplyNormalAttackDamage2() {
        List<DamageablePart> damageableList = overlap2.GetOverlapDamageableParts();
        foreach (DamageablePart damageable in damageableList) {
            ImpactEffectSystem impact = GameManager.SpawnImpactEffect(impact2);
            impact.transform.position = damageable.Center;
            if (transform.localScale.x < 0) impact.Flip();
        }
        
        ApplyDamage(damageableList, new Damage(this, damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right));
    }

    protected override void FinishNormalAttack() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
