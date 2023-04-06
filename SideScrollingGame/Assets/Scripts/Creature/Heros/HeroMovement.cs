using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : Movement
{   
    protected CapsuleCollider2D capCollider;
    protected Animator          anim;
    protected SpriteRenderer    rend;

    public delegate void MovementOperation(HeroMovement movement);
    public MovementOperation onJumpStart;
    public MovementOperation onBackToGround;
    public MovementOperation onRollStart;

    void Awake() {
        GrabNecessaryComponent();
        capCollider = GetComponent<CapsuleCollider2D>();
        anim        = GetComponent<Animator>();
        rend        = GetComponent<SpriteRenderer>();
    }

    void Update() {
        // GrabInputsByInputSystem();
        DefaultUpdate();

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", Mathf.Abs(horizInput) > 0);
    }

    void FixedUpdate() {
        ReachTargetSpeedByForce();
    }

    private void OnJumpStart() {
        anim.SetTrigger("jump");
        if (onJumpStart != null) onJumpStart(this);
    }

    private void OnBackToGround() {
        if (onBackToGround != null) onBackToGround(this);
    }   

    private void OnRollStart() {
        anim.SetTrigger("roll");
        if (onRollStart != null) onRollStart(this);
    }
}
