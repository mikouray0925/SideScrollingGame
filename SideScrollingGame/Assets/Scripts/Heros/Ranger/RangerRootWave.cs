using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerRootWave : MonoBehaviour
{
    [Header ("Speed multiplier")]
    [SerializeField] string speedMultiplierTag;
    [SerializeField] float speedMultiplier;

    [Header ("Damage")]
    public Overlap damageOverlap;
    public Damage damage;

    [Header ("Cooldown")]
    public CooldownSystem cooldown;

    [Header ("Ground")]
    [SerializeField] Transform[] groundCheckPoints;

    Animator anim;
    Collider2D col;
    HashSet<Movement> affectedMovements = new HashSet<Movement>();
    
    private void Awake() {
        anim = GetComponent<Animator>();
        col  = GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }

    public bool IsActive {
        get {
            return gameObject.activeSelf;
        }
        set {
            if(value && !gameObject.activeSelf) Activate();
            else if (!value && gameObject.activeSelf) Deactivate();
        }
    }

    public void Activate() { 
        if (!AbleToActivate()) return;
        gameObject.SetActive(true);
        anim.SetTrigger("restart");
    }

    public bool AbleToActivate() {
        return !cooldown.IsInCD && HaveValidGroundSpace();
    }

    public bool HaveValidGroundSpace() {
        foreach (Transform point in groundCheckPoints) {
            RaycastHit2D hitInfo = Physics2D.Raycast(point.position, Vector2.down, 0.1f, GlobalSettings.obstacleLayers);
            if (!hitInfo.collider) return false;
        }
        return true; 
    }

    private void ApplyDamage() {
        HashSet<Health> healthSet = damageOverlap.GetOverlapHealthComponents();
        foreach (Health health in healthSet) {
            health.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.TryGetComponent<Movement>(out Movement movement)) {
            affectedMovements.Add(movement);
            movement.speedData.multiplier.AddMultiplier(speedMultiplier, speedMultiplierTag);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.TryGetComponent<Movement>(out Movement movement)) {
            affectedMovements.Remove(movement);
            movement.speedData.multiplier.RemoveMultiplier(speedMultiplierTag);
        }
    }

    public void Deactivate() {
        foreach (Movement movement in affectedMovements) {
            movement.speedData.multiplier.RemoveMultiplier(speedMultiplierTag);
        }
        affectedMovements.Clear();
        gameObject.SetActive(false);
        cooldown.StartCooldownCoroutine();
    }
}
