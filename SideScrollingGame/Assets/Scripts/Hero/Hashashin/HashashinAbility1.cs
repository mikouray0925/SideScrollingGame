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
    
    Movement movement;
    Animator anim;

    bool attackIsInCD;
    Vector2 startPos;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetButtonDown("Fire2")) UnleashAbility1();
    }

    public bool UnleashAbility1() {
        damageableList = rangeOverlap.GetOverlapDamageableParts();
        if (AbleToAttack() && damageableList.Count > 0) {
            anim.SetTrigger("ability1");
            movement.LockMovementForSeconds(2.3f);
            movement.Brake();
            startPos = self.Center;
            movement.gravityScale = 0;
            self.health.isInvincible = true;
            Invoke(nameof(ResetEverything), 2.3f);
            attackIsInCD = true;
            Invoke(nameof(FinishAttackCD), AttackCD);
            return true;
        } else {
            return false;
        }
    }

    private void ApplyAbility1Damage(int hitCount) {
        switch (hitCount) {
        case 0:
            ApplyDamage(damageOverlap, Vector2.down);
            break;
        case 1:
            ApplyDamage(damageOverlap, Mathf.Sign(transform.localScale.x) * Vector2.left);
            break;
        case 2:
            ApplyDamage(damageOverlap, Mathf.Sign(transform.localScale.x) * Vector2.right);
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
        ResetEverything();
    }

    private void ResetEverything() {
        movement.UnlockMovement();
        movement.gravityScale = 1f;
        self.health.isInvincible = false;
    }

    private void FinishAttackCD() {
        attackIsInCD = false;
    }

    public bool AbleToAttack() {
        return !attackIsInCD && movement.isGrounded && !movement.isJumping && !movement.isRolling;
    }
}
