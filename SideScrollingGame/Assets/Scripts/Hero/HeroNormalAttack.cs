using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNormalAttack : Attack
{
    [SerializeField] Overlap overlap;
    
    Movement movement;
    Animator anim;
    bool attackIsInCD;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetButtonDown("Fire1")) UnleashNormalAttack();
    }

    public bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(0.4f);
            attackIsInCD = true;
            Invoke(nameof(FinishAttackCD), AttackCD);
            return true;
        } else {
            return false;
        }
    }

    private void ApplyNormalAttackDamage() {
        HashSet<Health> healthSet = overlap.GetOverlapHealthComponents();
        foreach (Health health in healthSet) {
            health.TakeDamage(attackData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right);
        }
        healthSet.Clear();
    }

    private void FinishNormalAttack() {
        movement.UnlockMovement();
    }

    private void FinishAttackCD() {
        attackIsInCD = false;
    }

    public bool AbleToAttack() {
        return !attackIsInCD && movement.isGrounded && !movement.isJumping && !movement.isRolling;
    }
}
