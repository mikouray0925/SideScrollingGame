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
        if (Input.GetButtonDown("Fire3")) UnleashAbility2();
    }

    public virtual bool UnleashAbility2() {
        return false;
    }

    protected virtual void FinishAbility2() {}
}
