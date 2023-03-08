using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: Do not add any transform parent to this gameObject.
public class RangerBeam : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] float spriteLength;

    [Header ("References")]
    [SerializeField] Animator anim;
    [SerializeField] OverlapCircle sourceOverlap;
    [SerializeField] OverlapBox beamOverlap;
    [SerializeField] CompositeOverlap damageOverlap;

    public Damage damage;

    public void Activate(float firingSide, float length, Vector2 pos, Damage _damage) {
        Vector3 tempVec = transform.localScale;
        tempVec.x = firingSide * (length / spriteLength);
        transform.localScale = tempVec;
        beamOverlap.size.x = length;

        transform.position = pos + (firingSide * length / 2f) * Vector2.right;
        
        damage = _damage;

        gameObject.SetActive(true);
        anim.SetTrigger("restart");
    }

    private void ApplyDamage() {
        HashSet<Health> healthSet = damageOverlap.GetOverlapHealthComponents();
        foreach (Health health in healthSet) {
            health.TakeDamage(damage);
        }
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }

    public void Flip() {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y , transform.localScale.z);
    }
}
