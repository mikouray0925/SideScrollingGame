using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAbility1 : Attack
{
    protected Movement movement;

    protected virtual void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    protected virtual void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility1();
    }

    public virtual bool UnleashAbility1() {
        return false;
    }

    protected void Ability1AnimStartEvent() {
        isAttacking = true;
    }

    public virtual void ButtonReleaseAction() {}

    protected virtual void FinishAbility1() {}
}
