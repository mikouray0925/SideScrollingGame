using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNormalAttack : Attack
{
    [SerializeField] Overlap overlap;
    
    Movement movement;
    Animator anim;
    bool attackIsInCD;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    public bool UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.LockMovementForSeconds(0.4f);
            movement.Brake();
            attackIsInCD = true;
            Invoke(nameof(FinishAttackCD), AttackCD);
            return true;
        } else {
            return false;
        }
    }

    private void ApplyNormalAttackDamage() {
        ApplyDamage(overlap, Mathf.Sign(transform.localScale.x) * Vector2.right);
    }

    private void FinishNormalAttack() {
        movement.UnlockMovement();
    }

    private void FinishAttackCD() {
        attackIsInCD = false;
    }

    public bool AbleToAttack() {
        return !attackIsInCD && movement.isGrounded;
    }
}
