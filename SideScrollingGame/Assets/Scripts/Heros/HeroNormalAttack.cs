using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNormalAttack : Attack
{
    public virtual bool UnleashNormalAttack() {
        return false;
    }

    protected void NormalAttackAnimStartEvent() {
        isAttacking = true;
    }

    public virtual void ButtonReleaseAction() {}

    protected virtual void FinishNormalAttack() {}
}
