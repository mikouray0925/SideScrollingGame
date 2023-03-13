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
        FinishAttackIfAnimNotPlaying();
    }

    public virtual void UnleashAbility2() {}

    protected void Ability2AnimStartEvent() {
        AttackAnimStart();
    }

    public virtual void ButtonReleaseAction() {}
}
