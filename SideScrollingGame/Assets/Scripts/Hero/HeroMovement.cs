using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : Movement
{   
    protected CapsuleCollider2D capCollider;
    protected Animator          anim;
    protected SpriteRenderer    rend;

    void Awake() {
        GrabNecessaryComponent();
        capCollider = GetComponent<CapsuleCollider2D>();
        anim        = GetComponent<Animator>();
        rend        = GetComponent<SpriteRenderer>();
    }

    void Update() {
        GrabInputsByInputSystem();
        FlipWithHorizInput();
        CheckGround();
        ApplyFallingGravity();

        if (Input.GetButtonDown("Jump")) {
            if (Jump()) anim.SetTrigger("jump");
        } 
        if (Input.GetButtonUp("Jump")) {
            CutoffJump();
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", Mathf.Abs(horizInput) > 0);
    }

    void FixedUpdate() {
        ReachTargetSpeedByForce();
    }
}
