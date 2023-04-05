using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header ("Basic")]
    [SerializeField] public DamageData damageData;
    [SerializeField] public CooldownSystem attackCD;

    [Header ("Animation")]
    [SerializeField] protected Animator anim;
    [SerializeField] protected string attackClipName;

    //|=========================================================
    //| Set "isAttacking" to true, when you want other scripts 
    //| to know this creature is performing this <Attack>.
    //| Then other script won't do things that should not
    //| override this action. 
    //|=========================================================
    public bool isAttacking {get; private set;}

    //|=========================================================
    //| Check every condition related to this <Attack> wether
    //| it allow this <Attack> to perform.
    //| Put every condition check in this function.
    //| 
    //|=========================================================
    public virtual bool AbleToAttack() {
        return !isAttacking && !attackCD.IsInCD;
    }

    //|=========================================================
    //| We often want to call "FinishAttack()" function when 
    //| attack animation finishes. I implemented this by 
    //| comparing the names of ".anim". This is the only I found
    //| out to do this.
    //|=========================================================
    public virtual bool IsPlayingAttackAnimClip() {
        return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == attackClipName;
    }

    //|=========================================================
    //| This is an event which should be put at the begining of  
    //| the animation. Do not call this in script because when 
    //| we start an animation by script, it may not actual start.
    //| 
    //|=========================================================
    protected void AttackAnimStart() {
        isAttacking = true;
    }

    //|=========================================================
    //| Put this function into Update().
    //| "isAttack" shold be reset to false at some moment.
    //| It's a good timing when attack anim finishes.
    //| Also use a func "FinishAttack' to end is a good choice.
    //|=========================================================
    protected void FinishAttackIfAnimNotPlaying() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAttack();
    }

    //|=========================================================
    //| Set "isAttacking" to false, then call "OnAttackFinish" 
    //| to handle the last stage of this attack.
    //| 
    //| 
    //|=========================================================
    private void FinishAttack() {
        isAttacking = false;
        OnAttackFinish();
    }

    //|=========================================================
    //| Handle the last stage of this attack in this func.
    //| 
    //| 
    //| 
    //|=========================================================
    protected virtual void OnAttackFinish() {}

    //|=========================================================
    //| Let every <Health> component in "overlap" take damage.
    //| Return how many <Health> take damage.
    //| damage = Damage * damageMultiplier + damageAddend
    //| "direction" is the direction where the damage go.
    //|=========================================================
    protected int ApplyDamage(Overlap overlap, Damage damageInfo) {
        return ApplyDamage(overlap.GetOverlapHealthComponents(), damageInfo);
    }
    
    //|=========================================================
    //| Let every <Health> component in "damageableParts" take damage.
    //| Return how many <Health> take damage.
    //| If you want to add force to <DamageablePart>, this is 
    //| better.
    //|=========================================================
    protected int ApplyDamage(List<DamageablePart> damageableParts, Damage damageInfo) {
        return ApplyDamage(DamageablePart.GetHealthComponents(damageableParts), damageInfo);
    }

    //|=========================================================
    //| Let every <Health> component in "healthSet" take damage.
    //| Return how many <Health> take damage.
    //| The base function of another two.
    //| 
    //|=========================================================
    protected int ApplyDamage(HashSet<Health> healthSet, Damage damageInfo) {
        if (!damageInfo.attacker) damageInfo.attacker = transform;
        if (!damageInfo.attack) damageInfo.attack = this;
        if (damageInfo.forces == null) damageInfo.forces = new List<Damage.Force>();
        damageInfo.mainDirection.Normalize();

        foreach (Health health in healthSet) {
            health.TakeDamage(damageInfo);
        }
        return healthSet.Count;
    }
}
