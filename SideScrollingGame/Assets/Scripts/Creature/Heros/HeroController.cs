using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroController : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PlayerInput input;
    [SerializeField] HeroBrain initBindingHero;
    public HeroBrain bindingHero {get; private set;}

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

    public void MakeCameraFollowHero(CameraFollow camFollow) {
        camFollow.followingTarget = bindingHero.movement.transform;
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

        input.actions["Ability1"].started  += Ability1Action;
        input.actions["Ability1"].canceled += Ability1Action;
        input.actions["Ability2"].started  += Ability2Action;
        input.actions["Ability2"].canceled += Ability2Action;

        input.actions["PickItemDrop"].started += cxt => hero.picker.PickNearestItem();
        
        bindingHero = hero;
    }

    public void Unbind() { 
        if (!bindingHero) return;

        input.actions["Jump"].started  -= cxt => bindingHero.movement.Jump();
        input.actions["Jump"].canceled -= cxt => bindingHero.movement.CutoffJump();
        input.actions["Roll"].started  -= cxt => bindingHero.movement.Roll();

        input.actions["NormalAttack"].started  -= cxt => bindingHero.normalAttack.UnleashNormalAttack();
        input.actions["NormalAttack"].canceled -= cxt => bindingHero.normalAttack.ButtonReleaseAction();

        input.actions["Ability1"].started  -= Ability1Action;
        input.actions["Ability1"].canceled -= Ability1Action;
        input.actions["Ability2"].started  -= Ability2Action;
        input.actions["Ability2"].canceled -= Ability2Action;

        input.actions["PickItemDrop"].started -= cxt => bindingHero.picker.PickNearestItem();

        bindingHero = null;
    }

    private void Ability1Action(InputAction.CallbackContext ctx) {
        if (ctx.started) bindingHero.ability1.UnleashAbility1();
        else if (ctx.canceled) bindingHero.ability1.ButtonReleaseAction();
    }

    private void Ability2Action(InputAction.CallbackContext ctx) {
        if (ctx.started) bindingHero.ability2.UnleashAbility2();
        else if (ctx.canceled) bindingHero.ability2.ButtonReleaseAction();
    }
}
