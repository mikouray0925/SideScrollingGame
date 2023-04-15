using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinAbility2 : HeroAbility2
{
    [Header ("Parameters")]
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] float tornadoStartSpeed;

    [Header ("References")]
    [SerializeField] HashashinTornado tornado;
    [SerializeField] Transform tornadoStartPoint;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip windSFX;
    
    public override bool AbleToAttack() {
        return 
        !isAttacking &&
        !attackCD.IsInCD && 
        movement.isGrounded && 
        !movement.isJumping && 
        !movement.isRolling;
    }

    public override void UnleashAbility2() {
        if (AbleToAttack()) {
            anim.SetTrigger("ability2");
            movement.movementLock.AddLock("ability2", 1f);
            movement.Brake();
        } 
    }

    private void ReleaseTornado() {
        float facingSide = Mathf.Sign(transform.localScale.x);
        Damage damageInfo = new Damage(this, damageData.Damage * damageMultiplier, facingSide * Vector2.right);
        damageInfo.forces.Add(new Damage.Force(Vector2.right, ForceMode2D.Force));
        audioSource.PlayOneShot(windSFX, AudioManager.effectVolume);
        tornado.Activate(tornadoStartPoint.position, facingSide, tornadoStartSpeed, damageInfo);
    }

    protected override void OnAttackFinish() {
        attackCD.StartCooldownCoroutine();
        movement.movementLock.RemoveLock("ability2");
    }
}
