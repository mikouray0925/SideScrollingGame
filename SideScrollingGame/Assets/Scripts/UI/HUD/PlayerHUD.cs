using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : InterfaceUI
{
    [Header ("References")]
    [SerializeField] HealthBar healthBar;
    [SerializeField] AttackInfoDisplay ability1;
    [SerializeField] AttackInfoDisplay ability2;
    
    public HeroBrain bindingHero {get; private set;}

    public void BindLocalHero() {
        Bind(AppManager.instance.LocalHero);
    }

    public void Bind(HeroBrain hero) {
        healthBar.health = hero.health;
        ability1.atk = hero.ability1;
        ability2.atk = hero.ability2;
        bindingHero = hero;
    }

    public void Unbind() {
        ability1.atk = null;
        ability2.atk = null;
        bindingHero = null;
    }
}
