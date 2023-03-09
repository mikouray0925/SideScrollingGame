using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerNormalAttack : HeroNormalAttack
{
    [Header ("Arrow")]
    [SerializeField] ProjectilePool arrowPool;
    [SerializeField] Transform arrowStartPoint;
    [SerializeField] float arrowStartSpeed;
    
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
            base.AbleToAttack() && 
            movement.isGrounded && 
            !movement.isJumping && 
            !movement.isRolling;
    }

    protected override bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(1.1f);
            movement.Brake();
            isAttacking = true;
        } 
        return isAttacking;
    }

    private void LaunchArrow() {
        Projectile arrow = arrowPool.GetInactiveProjectile();
        arrow.Activate();
        arrow.transform.position = arrowStartPoint.position;
        float facingSide = Mathf.Sign(transform.localScale.x);
        arrow.transform.localRotation = Quaternion.identity;
        if (Mathf.Sign(arrow.transform.localScale.x) != facingSide) {
            arrow.transform.localScale = new Vector3(
                -arrow.transform.localScale.x,
                 arrow.transform.localScale.y,
                 arrow.transform.localScale.z
            );
        }
        arrow.rb.velocity = facingSide * arrowStartSpeed * Vector2.right;
        arrow.damage = new Damage(this, damageData.Damage, Vector2.right);
        arrow.Launch();
    }

    protected override void FinishNormalAttack() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
