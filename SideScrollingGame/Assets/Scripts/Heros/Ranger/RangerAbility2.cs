using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAbility2 : HeroAbility2
{
    [Header ("paramters")]
    [SerializeField] float baseLength;
    [SerializeField] float elongationRate;
    [SerializeField] float damageMultiplierRate;
    [SerializeField] float basePower;
    [SerializeField] float powerIncreaseRate;
    
    [Header ("animations")]
    [SerializeField] string readyClipName;
    [SerializeField] string fireClipName;

    [Header ("References")]
    [SerializeField] Transform firePoint;
    [SerializeField] RangerBeam beam;

    bool isCharging;
    bool buttonReleased;
    float chargingStartTime;

    protected override void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility2();
        if (isCharging && buttonReleased) {
            Fire();
        }
    }

    public virtual void OnButtonRelease() {
        buttonReleased = true;
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
        } 
        buttonReleased = false;
        return isAttacking;
    }

    private void StartCharging() {
        if (isAttacking && !isCharging) {
            chargingStartTime = Time.time;
            isCharging = true;
        }
    }

    public override void ButtonReleaseAction() {
        buttonReleased = true;
    }

    private void Fire() {
        if (isAttacking && isCharging) {
            isCharging = false;
            anim.SetTrigger("ability2Fire");

            float firingSide = Mathf.Sign(transform.localScale.x);
            float chargingTime = Time.time - chargingStartTime;

            Damage damageInfo = new Damage(this, damageData.Damage, firingSide * Vector2.right);
            damageInfo.damage *= 1f + damageMultiplierRate;

            Damage.Force force = new Damage.Force(
                firingSide * (basePower + powerIncreaseRate * chargingTime) * Vector2.right,
                ForceMode2D.Force
            );
            damageInfo.forces.Add(force);

            beam.Activate(firingSide, baseLength + chargingTime * elongationRate,
                          firePoint.position, damageInfo);
        }
    }

    protected override void FinishAbility2() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }
}
