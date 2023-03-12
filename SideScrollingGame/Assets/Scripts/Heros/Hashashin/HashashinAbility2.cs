using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinAbility2 : HeroAbility2
{
    [Header ("Parameters")]
    [SerializeField] float tornadoStartSpeed;

    [Header ("References")]
    [SerializeField] HashashinTornado tornado;
    [SerializeField] Transform tornadoStartPoint;
    
    public override bool AbleToAttack() {
        return 
        !isAttacking &&
        !attackCD.IsInCD && 
        movement.isGrounded && 
        !movement.isJumping && 
        !movement.isRolling;
    }

    public override bool UnleashAbility2() {
        if (AbleToAttack()) {
            anim.SetTrigger("ability2");
            movement.LockMovementForSeconds(1f);
            movement.Brake();
        } 
        return isAttacking;
    }

    private void ReleaseTornado() {
        float facingSide = Mathf.Sign(transform.localScale.x);
        Damage damageInfo = new Damage(this, damageData.Damage, facingSide * Vector2.right);
        damageInfo.forces.Add(new Damage.Force(Vector2.right, ForceMode2D.Force));
        tornado.Activate(tornadoStartPoint.position, facingSide, tornadoStartSpeed, damageInfo);
    }

    protected override void FinishAbility2() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
