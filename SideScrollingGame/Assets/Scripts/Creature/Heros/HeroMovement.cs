using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : Movement
{   
    [Header ("SFX")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip jumpSFX;
    [SerializeField] AudioClip landSFX;
    [SerializeField] AudioClip rollSFX;
    
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
        audioSource.PlayOneShot(jumpSFX, AudioManager.effectVolume);
        if (onJumpStart != null) onJumpStart(this);
    }

    private void OnBackToGround() {
        audioSource.PlayOneShot(landSFX, AudioManager.effectVolume);
        if (onBackToGround != null) onBackToGround(this);
    }   

    private void OnRollStart() {
        anim.SetTrigger("roll");
        audioSource.PlayOneShot(rollSFX, AudioManager.effectVolume);
        if (onRollStart != null) onRollStart(this);
    }
}
