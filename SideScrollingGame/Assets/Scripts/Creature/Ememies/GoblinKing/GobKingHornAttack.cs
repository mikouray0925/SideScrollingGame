using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobKingHornAttack : Attack
{
    [Header ("Horn")]
    [SerializeField] Overlap overlap;
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] float damageHorizForce = 0;
    [SerializeField] float damageUpForce = 0;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip swingSFX;

    Movement movement;
    
    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        FinishAttackIfAnimNotPlaying();
    }

    public override bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD && movement.isGrounded;
    }

    public void UnleashHornAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("hornAttack");
            movement.movementLock.AddLock("hornAttack", 0.25f);
            movement.Brake();
        } 
    }

    private void HornAttackAnimStartEvent() {
        AttackAnimStart();
    }

    private void PlayHornSwingSFX() {
        audioSource.PlayOneShot(swingSFX, AudioManager.effectVolume);
    }

    private void ApplyHornAttackDamage() {
        Damage damage = new Damage(this, damageMultiplier * damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right);
        Vector2 force = Mathf.Sign(transform.localScale.x) * damageHorizForce * Vector2.right + damageUpForce * Vector2.up;
        damage.forces.Add(new Damage.Force(force, ForceMode2D.Force));
        ApplyDamage(overlap, damage);
    }

    protected override void OnAttackFinish() {
        movement.movementLock.RemoveLock("hornAttack");
        attackCD.StartCooldownCoroutine();
    }
}
