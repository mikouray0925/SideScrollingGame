using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashashinAbility1 : Attack
{
    [Header ("Overlaps")]
    [SerializeField] Overlap rangeOverlap;
    [SerializeField] Overlap damageOverlap;

    [Header ("Damageable")]
    [SerializeField] DamageablePart self;
    [SerializeField] List<DamageablePart> damageableList;

    [Header ("End")]
    [SerializeField] string attackClip2Name;
    
    Movement movement;

    Vector2 startPos;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility1();
        if (Input.GetButtonDown("Fire2")) UnleashAbility1();
    }

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

    public bool UnleashAbility1() {
        damageableList = rangeOverlap.GetOverlapDamageableParts();
        if (AbleToAttack() && damageableList.Count > 0) {
            anim.SetTrigger("ability1");
            movement.LockMovementForSeconds(2.3f);
            movement.Brake();
            movement.gravityScale = 0;
            self.health.isInvincible = true;
            startPos = self.Center;
            isAttacking = true;
        } 
        return isAttacking;
    }

    private void ApplyAbility1Damage(int hitCount) {
        Damage damageInfo = new Damage(this, damageData.Damage, Vector2.zero);

        switch (hitCount) {
        case 0:
            damageInfo.mainDirection = Vector2.down;
            ApplyDamage(damageOverlap, damageInfo);
            break;
        case 1:
            damageInfo.mainDirection = Mathf.Sign(transform.localScale.x) * Vector2.left;
            ApplyDamage(damageOverlap, damageInfo);
            break;
        case 2:
            damageInfo.mainDirection = Mathf.Sign(transform.localScale.x) * Vector2.right;
            ApplyDamage(damageOverlap, damageInfo);
            break;
        }   
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

    private void FinishAbility1() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        movement.gravityScale = 1f;
        self.health.isInvincible = false;
        isAttacking = false;
    }
}
