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
        if (Input.GetButtonDown("Fire2")) UnleashAbility1();
    }

    public virtual bool UnleashAbility1() {
        return false;
    }

    protected virtual void FinishAbility1() {}
}
