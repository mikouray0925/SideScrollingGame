using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header ("Basic")]
    [SerializeField] public DamageData damageData;
    [SerializeField] public CooldownSystem attackCD;
    public bool isAttacking {get; protected set;}

    [Header ("Animation")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected string attackClipName;

    public virtual bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD;
    }

    public virtual bool IsPlayingAttackAnimClip() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == attackClipName;
    }

    //|=========================================================
    //| Let every <Health> component in "overlap" take damage.
    //| Return how many <Health> take damage.
    //| damage = Damage * damageMultiplier + damageAddend
    //| "direction" is the direction where the damage go.
    //|=========================================================
    protected int ApplyDamage(Overlap overlap, Vector2 direction, float damageMultiplier = 1f, float damageAddend = 0) {
        return ApplyDamage(overlap.GetOverlapHealthComponents(), direction, damageMultiplier, damageAddend);
    }
    
    //|=========================================================
    //| Let every <Health> component in "damageableParts" take damage.
    //| Return how many <Health> take damage.
    //| If you want to add force to <DamageablePart>, this is 
    //| better.
    //|=========================================================
    protected int ApplyDamage(List<DamageablePart> damageableParts, Vector2 direction, float damageMultiplier = 1f, float damageAddend = 0) {
        return ApplyDamage(DamageablePart.GetHealthComponents(damageableParts), direction, damageMultiplier, damageAddend);
    }

    //|=========================================================
    //| Let every <Health> component in "healthSet" take damage.
    //| Return how many <Health> take damage.
    //| The base function of another two.
    //| 
    //|=========================================================
    protected int ApplyDamage(HashSet<Health> healthSet, Vector2 direction, float damageMultiplier = 1f, float damageAddend = 0) {
        foreach (Health health in healthSet) {
            health.TakeDamage(damageData.Damage * damageMultiplier + damageAddend, direction);
        }
        return healthSet.Count;
    }
}
