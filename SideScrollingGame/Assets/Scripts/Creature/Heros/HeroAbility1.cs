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
        FinishAttackIfAnimNotPlaying();
    }

    public virtual void UnleashAbility1() {}

    protected void Ability1AnimStartEvent() {
        AttackAnimStart();
    }

    public virtual void ButtonReleaseAction() {}
}
