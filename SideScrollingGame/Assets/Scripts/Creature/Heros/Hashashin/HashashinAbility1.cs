using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinAbility1 : HeroAbility1
{
    [Header ("Multiplier")]
    [SerializeField] float damageMultiplier = 1f;
    
    [Header ("Overlaps")]
    [SerializeField] Overlap rangeOverlap;
    [SerializeField] Overlap damageOverlap;

    [Header ("Damageable")]
    [SerializeField] DamageablePart self;
    [SerializeField] List<DamageablePart> damageableList;

    [Header ("End")]
    [SerializeField] string attackClip2Name;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip teleportSFX;
    [SerializeField] AudioClip cutSFX;

    Vector2 startPos;

    public override bool IsPlayingAttackAnimClip() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == attackClipName || 
               anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == attackClip2Name;
    }

    public override bool AbleToAttack() {
        return 
        !isAttacking &&
        !attackCD.IsInCD && 
        movement.isGrounded && 
        !movement.isJumping && 
        !movement.isRolling;
    }

    public override void UnleashAbility1() {
        damageableList = rangeOverlap.GetOverlapDamageableParts();
        if (AbleToAttack() && damageableList.Count > 0) {
            anim.SetTrigger("ability1");
            movement.movementLock.AddLock("ability1", 2.3f);
            movement.Brake();
            movement.gravityScale = 0;
            self.health.isInvincible = true;
            startPos = self.Center;
        } 
    }

    private void ApplyAbility1Damage(int hitCount) {
        Damage damageInfo = new Damage(this, damageData.Damage * damageMultiplier, Vector2.zero);

        switch (hitCount) {
        case 0:
            damageInfo.mainDirection = Vector2.down;
            if (ApplyDamage(damageOverlap, damageInfo) > 0) {
                audioSource.PlayOneShot(cutSFX, AudioManager.effectVolume);
            }
            break;
        case 1:
            damageInfo.mainDirection = Mathf.Sign(transform.localScale.x) * Vector2.left;
            if (ApplyDamage(damageOverlap, damageInfo) > 0) {
                audioSource.PlayOneShot(cutSFX, AudioManager.effectVolume);
            }
            break;
        case 2:
            damageInfo.mainDirection = Mathf.Sign(transform.localScale.x) * Vector2.right;
            if (ApplyDamage(damageOverlap, damageInfo) > 0) {
                audioSource.PlayOneShot(cutSFX, AudioManager.effectVolume);
            }
            break;
        }   
    }

    private void PlayTeleportSFX() {
        audioSource.PlayOneShot(teleportSFX, AudioManager.effectVolume);
    }

    private void TeleportToNext(int hitCount) {
        if (hitCount < damageableList.Count) {
            self.Center = damageableList[hitCount].Center;
        } else {
            TeleportBack();
        }
    }

    private void TeleportBack() {
        self.Center = startPos;
        anim.SetTrigger("ability1End");
    }

    protected override void OnAttackFinish() {
        attackCD.StartCooldownCoroutine();
        movement.movementLock.RemoveLock("ability1");
        movement.gravityScale = 1f;
        self.health.isInvincible = false;
    }
}
