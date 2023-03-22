using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PlayerInput input;
    [SerializeField] HeroBrain initBindingHero;
    HeroBrain bindingHero;

    void Awake() {
        if (initBindingHero) Bind(initBindingHero);
    }

    void Update() {
        if (bindingHero) {
            bindingHero.movement.horizInput = input.actions["Move"].ReadValue<float>();
        }
    }

    public void SetHeroPosTo(Vector3 toPos) {
        bindingHero.movement.transform.position = toPos;
    }

    public bool IsBinded() {
        return bindingHero != null;
    }

    public void Bind(HeroBrain hero) {
        input.actions["Jump"].started  += cxt => hero.movement.Jump();
        input.actions["Jump"].canceled += cxt => hero.movement.CutoffJump();
        input.actions["Roll"].started  += cxt => hero.movement.Roll();

        input.actions["NormalAttack"].started  += cxt => hero.normalAttack.UnleashNormalAttack();
        input.actions["NormalAttack"].canceled += cxt => hero.normalAttack.ButtonReleaseAction();

        input.actions["Ability1"].started  += cxt => hero.ability1.UnleashAbility1();
        input.actions["Ability1"].canceled += cxt => hero.ability1.ButtonReleaseAction();
        input.actions["Ability2"].started  += cxt => hero.ability2.UnleashAbility2();
        input.actions["Ability2"].canceled += cxt => hero.ability2.ButtonReleaseAction();
        
        bindingHero = hero;
    }

    void Unbind() {

    }
}
