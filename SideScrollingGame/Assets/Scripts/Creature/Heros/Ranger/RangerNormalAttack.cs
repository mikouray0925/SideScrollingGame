using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerNormalAttack : HeroNormalAttack
{   
    [Header ("Arrow")]
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform arrowHolder;
    [SerializeField] Transform arrowStartPoint;
    [SerializeField] float arrowStartSpeed;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip bowReleaseSFX;

    ObjPool<Arrow> arrowPool;

    protected override void Awake() {
        base.Awake();
        arrowPool = new ObjPool<Arrow>(arrowPrefab, arrowHolder, 5);
        arrowPool.onGet += SetupArrow;
    }

    public override bool AbleToAttack() {
        return 
            base.AbleToAttack() && 
            movement.isGrounded && 
            !movement.isJumping && 
            !movement.isRolling;
    }

    public override void UnleashNormalAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("normalAttack");
            movement.movementLock.AddLock("normalAttack", 1.1f);
            movement.Brake();
        } 
    }

    private void PlayBowReleaseSFX() {
        audioSource.PlayOneShot(bowReleaseSFX, AudioManager.effectVolume);
    }

    private void SetupArrow(Arrow arrow) {
        arrow.inPool = arrowPool;
        arrow.transform.position = arrowStartPoint.position;
        float facingSide = Mathf.Sign(transform.localScale.x);
        arrow.transform.localRotation = Quaternion.identity;
        if (Mathf.Sign(arrow.transform.localScale.x) != facingSide) {
            arrow.transform.localScale = new Vector3(
                -arrow.transform.localScale.x,
                 arrow.transform.localScale.y,
                 arrow.transform.localScale.z
            );
        }
        arrow.rb.velocity = facingSide * arrowStartSpeed * Vector2.right;
        arrow.damage = new Damage(this, damageData.Damage, facingSide * Vector2.right);
    }

    private void LaunchArrow() {
        arrowPool.Get().Launch();
    }

    protected override void OnAttackFinish() {
        attackCD.StartCooldownCoroutine();
        movement.movementLock.RemoveLock("normalAttack");
    }
}
