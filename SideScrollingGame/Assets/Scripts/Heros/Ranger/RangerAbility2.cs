using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAbility2 : HeroAbility2
{
    [Header ("animations")]
    [SerializeField] string readyClipName;
    [SerializeField] string fireClipName;

    bool isCharging;
    bool buttonReleased;
    float chargingStartTime;

    protected override void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility2();
        if (Input.GetButtonDown("Fire3")) {
            UnleashAbility2();
            buttonReleased = false;
        }
        if (Input.GetButtonUp("Fire3")) {
            buttonReleased = true;
        }
        if (isCharging && buttonReleased) {
            Fire();
        }
    }
    
    public override bool IsPlayingAttackAnimClip() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == readyClipName || 
               anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == fireClipName;
    }
    
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
            movement.LockMovementForSeconds(0.8f);
            movement.Brake();
            isAttacking = true;
        } 
        return isAttacking;
    }

    private void StartCharging() {
        if (isAttacking && !isCharging) {
            chargingStartTime = Time.time;
            isCharging = true;
        }
    }

    private void Fire() {
        if (isAttacking && isCharging) {
            isCharging = false;
            anim.SetTrigger("ability2Fire");
        }
    }

    protected override void FinishAbility2() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
