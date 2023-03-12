using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAbility2 : Attack
{
    protected Movement movement;

    protected virtual void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    protected virtual void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility2();
    }

    public virtual bool UnleashAbility2() {
        return false;
    }

    protected void Ability2AnimStartEvent() {
        isAttacking = true;
    }

    public virtual void ButtonReleaseAction() {}

    protected virtual void FinishAbility2() {}
}
