using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinNormalAttack : Attack
{
    [Header ("Overlap")]
    [SerializeField] Overlap overlap;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip swingSFX;
    [SerializeField] AudioClip cutSFX;
    
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

    public void UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.movementLock.AddLock("normalAttack", 0.4f);
            movement.Brake();
        } 
    }

    private void NormalAttackAnimStartEvent() {
        AttackAnimStart();
    }

    private void ApplyNormalAttackDamage() {
        if (ApplyDamage(overlap, new Damage(this, damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right)) > 0) {
            audioSource.PlayOneShot(cutSFX, AudioManager.effectVolume);
        } else {
            audioSource.PlayOneShot(swingSFX, AudioManager.effectVolume);
        }
    }

    protected override void OnAttackFinish() {
        movement.movementLock.RemoveLock("normalAttack");
        attackCD.StartCooldownCoroutine();
    }
}
