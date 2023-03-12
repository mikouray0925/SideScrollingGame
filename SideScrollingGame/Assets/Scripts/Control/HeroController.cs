using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    PlayerControls playerControls;
    
    [Header ("References")]
    [SerializeField] HeroBrain initBindingHero;
    HeroBrain bindingHero;

    void Awake() {
        playerControls = new PlayerControls();
        playerControls.Enable();
        if (initBindingHero) Bind(initBindingHero);
    }

    void Update() {
        if (bindingHero) {
            bindingHero.movement.horizInput = playerControls.Land.Move.ReadValue<float>();
        }
    }

    void Bind(HeroBrain hero) {
        playerControls.Land.Jump.started  += cxt => hero.movement.JumpAction();
        playerControls.Land.Jump.canceled += cxt => hero.movement.CutoffJump();
        playerControls.Land.Roll.started  += cxt => hero.movement.RollAction();

        playerControls.Land.NormalAttack.started  += cxt => hero.normalAttack.UnleashNormalAttack();
        playerControls.Land.NormalAttack.canceled += cxt => hero.normalAttack.ButtonReleaseAction();

        playerControls.Land.Ability1.started  += cxt => hero.ability1.UnleashAbility1();
        playerControls.Land.Ability1.canceled += cxt => hero.ability1.ButtonReleaseAction();
        playerControls.Land.Ability2.started  += cxt => hero.ability2.UnleashAbility2();
        playerControls.Land.Ability2.canceled += cxt => hero.ability2.ButtonReleaseAction();
        
        bindingHero = hero;
    }

    void Unbind() {

    }
}
