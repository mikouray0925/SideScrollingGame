using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroHealth : Health
{
    [Header ("Color")]
    [SerializeField] Color hurtColor;

    [Header ("Calling up")]
    [SerializeField] HeroBrain hero;
    
    Animator anim;
    Movement movement;
    
    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
    }

    protected void OnHealthDecrease(float deltaHealth) {
        MakeSpriteFlash(hurtColor, 0.1f);
    }

    private void OnLifeNumBecomeZero() {
        anim.SetTrigger("die");
        movement.horizInput = 0;
        movement.Brake();
        hero.movement.enabled = false;
        hero.normalAttack.enabled = false;
        hero.ability1.enabled = false;
        hero.ability2.enabled = false;
        hero.inventory.enabled = false;
    }

    private void ShowDeathEndGameWindow() {
        AppManager.instance.endGameWindow.text.text = "You Died";
        AppManager.instance.endGameWindow.Show();
    }

    protected override void ProcessDamage(Damage damageInfo, out float finalDamageVal) {
        anim.SetTrigger("takeHit");
        movement.Brake();
        movement.movementLock.AddLock("takeHit", 0.2f);
        base.ProcessDamage(damageInfo, out finalDamageVal);
    }

    private void OnReadData(PlayerData playerData) {
        SetHealthPropotion(playerData.healthProportion);
    }

    private void OnWriteData(PlayerData playerData) {
        playerData.healthProportion = Hp / MaxHp;
    }
}
