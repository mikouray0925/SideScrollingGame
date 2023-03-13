using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerNormalAttack : HeroNormalAttack
{
    [Header ("Arrow")]
    [SerializeField] ProjectilePool arrowPool;
    [SerializeField] Transform arrowStartPoint;
    [SerializeField] float arrowStartSpeed;

    public override bool AbleToAttack() {
        return 
            base.AbleToAttack() && 
            movement.isGrounded && 
            !movement.isJumping && 
            !movement.isRolling;
    }

    public override void UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(1.1f);
            movement.Brake();
        } 
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

    protected override void OnAttackFinish() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
    }
}
