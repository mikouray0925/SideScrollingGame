using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNormalAttack : Attack
{
    protected Movement movement;

    protected virtual void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    protected virtual void Update() {
        FinishAttackIfAnimNotPlaying();
    }
    
    public virtual void UnleashNormalAttack() {}

    protected void NormalAttackAnimStartEvent() {
        AttackAnimStart();
    }

    public virtual void ButtonReleaseAction() {}
}
