using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarDashAttack : Attack
{
    [Header ("Dash")]
    [SerializeField] AreaTrigger2D noseTrigger;
    [SerializeField] float speedMultiplier = 3.6f;
    [SerializeField] float damageMultiplier = 1f;
    [SerializeField] float damageForce = 0;

    HashSet<Health> damagedHealth = new HashSet<Health>();
    Movement movement;
    float dashDirection;
    
    private void Awake() {
        noseTrigger.onEnter_collider += OnNoseCollideWithOther;
        //noseTrigger.onStay_collider += OnNoseCollideWithOther;
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        FinishAttackIfAnimNotPlaying();
        if (isAttacking) {
            movement.horizInput = dashDirection;
        }
    }

    public override bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD && movement.isGrounded;
    }

    public void UnleashDashAttack() {
        if (AbleToAttack()) {
            anim.SetTrigger("startAttack");
            movement.speedData.multiplier.AddMultiplier(speedMultiplier, "dash");
            dashDirection = Mathf.Sign(transform.localScale.x);
            damagedHealth.Clear();
        } 
    }

    public void OnNoseCollideWithOther(Collider2D other) {
        if (!isAttacking) return;
        if (LayerUtil.Judge(other).IsInMask(GlobalSettings.obstacleLayers)) {
            FinishAttackAnim();
            movement.Flip();
        } 
        else if (LayerUtil.Judge(other).IsInMask(GlobalSettings.playerLayers)) {
            if (other.TryGetComponent<DamageablePart>(out DamageablePart damageable)) {
                if (damagedHealth.Contains(damageable.health)) return;
                Damage dashDamage = new Damage(this, damageMultiplier * damageData.Damage, Mathf.Sign(transform.localScale.x) * Vector2.right);
                dashDamage.forces.Add(new Damage.Force(Mathf.Sign(transform.localScale.x) * damageForce * Vector2.right, ForceMode2D.Force));
                damageable.health.TakeDamage(dashDamage);
                damagedHealth.Add(damageable.health);
            }
        }
    }

    public void FinishAttackAnim() {
        if (isAttacking) anim.SetTrigger("finishAttack");
    }

    private void DashAttackAnimStartEvent() {
        AttackAnimStart();
    }

    protected override void OnAttackFinish() {
        movement.speedData.multiplier.RemoveMultiplier("dash");
        attackCD.StartCooldownCoroutine();
    }
}
