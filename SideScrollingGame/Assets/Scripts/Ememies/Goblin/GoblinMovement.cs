using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinMovement : Movement
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
        DefaultUpdate();

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isRunning", Mathf.Abs(horizInput) > 0);
    }

    void FixedUpdate() {
        ReachTargetSpeedByForce();
    }
}
