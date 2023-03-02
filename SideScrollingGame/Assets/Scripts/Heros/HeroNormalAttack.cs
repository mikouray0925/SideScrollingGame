using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroNormalAttack : Attack
{
    protected virtual void Update() {
        if (Input.GetButtonDown("Fire1")) UnleashNormalAttack();
    }

    protected virtual bool UnleashNormalAttack() {
        return false;
    }

    protected virtual void FinishNormalAttack() {}
}
