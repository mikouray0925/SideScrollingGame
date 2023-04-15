using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobKingDashAttack : Attack
{
    [Header ("Dash")]
    [SerializeField] AreaTrigger2D headTrigger;
    [SerializeField] AreaTrigger2D spearTrigger;
    [SerializeField] float speedMultiplier = 3.6f;
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] float damageForce = 0;

    [Header ("Anim Clip Name")]
    [SerializeField] string sneerClipName;
    [SerializeField] string dashClipName;

    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip impactSFX;

    HashSet<Health> damagedHealth = new HashSet<Health>();
    Movement movement;
    float dashDirection;
    public bool isDashing {get; private set;} = false;
    
    private void Awake() {
        headTrigger.onEnter_collider += OnCollideWithOther;
        spearTrigger.onEnter_collider += OnCollideWithOther;
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        FinishAttackIfAnimNotPlaying();
        if (isDashing) {
            movement.horizInput = dashDirection;
        }
    }

    public override bool IsPlayingAttackAnimClip() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == sneerClipName ||
               anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == dashClipName;
    }

    public override bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD && movement.isGrounded;
    }

    public void UnleashDashAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("dashAttack");
            movement.movementLock.AddLock("dashAttack", 0.45f);
            movement.speedData.multiplier.AddMultiplier(speedMultiplier, "dash");
            dashDirection = Mathf.Sign(transform.localScale.x);
            damagedHealth.Clear();
        } 
    }

    public void StartDash() {
        if (isAttacking) isDashing = true;
    }

    public void OnCollideWithOther(Collider2D other) {
        if (!isAttacking) return;
        if (LayerUtil.Judge(other).IsInMask(GlobalSettings.obstacleLayers)) {
            FinishAttackAnim();
            movement.horizInput = 0;
            movement.Flip();
        } 
        else if (LayerUtil.Judge(other).IsInMask(GlobalSettings.playerLayers)) {
            if (other.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
                if (damagedHealth.Contains(damageable.health)) return;
                Damage dashDamage = new Damage(this, damageMultiplier * damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right);
                dashDamage.forces.Add(new Damage.Force(Mathf.Sign(transform.localScale.x) * damageForce * Vector2.right, ForceMode2D.Force));
                damageable.health.TakeDamage(dashDamage);
                audioSource.PlayOneShot(impactSFX, AudioManager.effectVolume);
                damagedHealth.Add(damageable.health);
            }
        }
    }

    public void FinishAttackAnim() {
        if (isAttacking) {
            isDashing = false;
            anim.SetTrigger("finishDash");
        }
    }

    private void DashAttackAnimStartEvent() {
        AttackAnimStart();
    }

    protected override void OnAttackFinish() {
        movement.speedData.multiplier.RemoveMultiplier("dash");
        attackCD.StartCooldownCoroutine();
        damagedHealth.Clear();
    }
}
