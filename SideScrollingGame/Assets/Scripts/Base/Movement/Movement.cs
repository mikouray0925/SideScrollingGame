using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    [Header ("Inputs")]
    [Range (-1f, 1f)]
    [SerializeField] public  float horizInput;
    [Range (-1f, 1f)]
    [SerializeField] public  float vertiInput;

    [Header ("Movement parameters")]
    [SerializeField] public    SpeedData speedData;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float accelerationPow;
    
    [Header ("Ground check")]
    [SerializeField] private   Vector2 groundCheckboxOffset;
    [SerializeField] private   Vector2 groundCheckboxSize;
    [SerializeField] protected bool drawGroundCheckbox;
    public  bool isGrounded {get; private set;}
    private bool avoidGrounded;

    [Header ("Jump parameters")]
    [SerializeField] protected Vector2 jumpForce;
    [SerializeField] public    float gravityScale;
    [SerializeField] protected float fallingGravityMultiplier;
    [SerializeField] private   float avoidGroundedTime;
    public  bool  isJumping {get; private set;}
    private bool  jumpCutApplied;

    [Header ("Rolling parameters")]
    [SerializeField] protected float rollingForce;
    [SerializeField] protected float rollingCD;
    [SerializeField] private   string rollingClipName;
    [SerializeField] private   Health health;
    public  bool  isRolling {get; private set;}
    private bool  isInInvinciblePeriod;
    private bool  rollingIsInCD;

    [Header ("References")]
    [SerializeField] private RectTransform canvasTransform;

    [Header ("Movement lock")]
    [SerializeField] private float lockMovementRemainingTime;
    public  bool  lockMovement {get; private set;}

    #region ComponentRef
    protected Rigidbody2D       rbody;
    
    #endregion

    #region Basic

    public void DefaultUpdate() {
        UpdateMovementLock();
        UpdateRolling();
        FlipWithHorizInput();
        CheckGround();
        ApplyFallingGravity();
    }

    protected void GrabNecessaryComponent()  {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmosSelected() {
        if (drawGroundCheckbox) Gizmos.DrawWireCube(GroundCheckboxCenter, groundCheckboxSize);
    }

    #endregion

    #region Control

    public void GrabInputsByInputSystem()  {
        horizInput = Input.GetAxis("Horizontal");
        vertiInput = Input.GetAxis("Vertical");
    }

    // inverse transform.localScale.x and canvasTransform.localScale.x
    public void Flip() {
        if (lockMovement) return;

        transform.localScale = new Vector3(
            -transform.localScale.x, 
             transform.localScale.y, 
             transform.localScale.z
        );
        if (canvasTransform && Mathf.Sign(transform.localScale.x) != Mathf.Sign(canvasTransform.localScale.x)) {
            canvasTransform.localScale = new Vector3(
                -canvasTransform.localScale.x, 
                 canvasTransform.localScale.y, 
                 canvasTransform.localScale.z
            );
        }
    }

    //|=========================================================
    //| This is a part of "Update" process.
    //| Call "Flip()" if Abs(horizInput) > 0 and 
    //| Sign(horizInput) != Sign(transform.localScale.x).
    //| 
    //|=========================================================
    protected void FlipWithHorizInput() {
        if (Mathf.Abs(horizInput) > 0 && Mathf.Sign(horizInput) != Mathf.Sign(transform.localScale.x)) {
            Flip();
        }
    }
    
    // Set rbody.velocity.x to 0.
    public void Brake() {
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    // unsafe
    public void BrakeByForce() {
        if (Mathf.Abs(rbody.velocity.x) < 0.1f) return;
        float signOfVelocity = Mathf.Sign(rbody.velocity.x);
        float maxBrakeForce = rbody.mass * MovementSpeed * -signOfVelocity;
        float brakeForce = rbody.mass * -rbody.velocity.x;
        if (signOfVelocity > 0 && brakeForce > maxBrakeForce) brakeForce = maxBrakeForce;
        if (signOfVelocity < 0 && brakeForce < maxBrakeForce) brakeForce = maxBrakeForce;
        rbody.AddForce(brakeForce * Vector2.right, ForceMode2D.Impulse);
    }

    #endregion

    #region Lock

    //|=========================================================
    //| An important part of movement "Update" process.
    //| Check wether MovementLock needs to be unlocked at every 
    //| frame. If yes, call "UnlockMovement()".
    //| 
    //|=========================================================
    protected void UpdateMovementLock() {
        if (lockMovementRemainingTime > 0) lockMovementRemainingTime -= Time.deltaTime;
        if (lockMovementRemainingTime > 0) {
            lockMovement = true;
        } else {
            UnlockMovement();
        }
    }

    //|=========================================================
    //| When movement is locked, the character cannot move,
    //| jump, and roll.
    //| 
    //| 
    //|=========================================================
    public void LockMovementForSeconds(float duration) {
        if (duration > lockMovementRemainingTime) {
            lockMovementRemainingTime = duration;
            lockMovement = true;
        }
    }

    public void UnlockMovement() {
        lockMovementRemainingTime = 0;
        lockMovement = false;
    }

    #endregion

    #region Ground
    
    public Vector2 GroundCheckboxCenter {
        get {
            Vector2 center = transform.position;
            return center + groundCheckboxOffset;
        }
        private set {}
    }

    //|=========================================================
    //| An important part of movement "Update" process.
    //| Check the character is grounded or not and store the
    //| result. The result is useful for jumping, rolling, and
    //| also Attacking.
    //|=========================================================
    protected bool CheckGround() {
        bool oldGroundedVal = isGrounded;
        if (avoidGrounded) {
            isGrounded = false;
        } else {
            isGrounded = IsGrounded(Vector2.zero);
        }
        if (!oldGroundedVal && isGrounded) BackToGround();
        return isGrounded;
    }

    //|=========================================================
    //| Castdown the "GroundCheckbox". If the box hit the ground
    //| layers, return true, else return false.
    //| 
    //| 
    //|=========================================================
    public bool IsGrounded(Vector2 offset, float downcastDistance = 0.1f) {
        Vector2 origin = GroundCheckboxCenter + offset;
        RaycastHit2D hit = Physics2D.BoxCast(origin, groundCheckboxSize, 0, Vector2.down, downcastDistance, GameManager.groundLayers);
        return (hit.collider != null && Vector2.Dot(hit.normal, Vector2.up) > 0);
    }

    //|=========================================================
    //| If the "isGrounded" value changes from false to true, 
    //| this will be called in "CheckGround" function.
    //| 
    //| 
    //|=========================================================
    private void BackToGround() {
        if (isJumping) ResetJump();
        WhenBackToGround();
    }
    protected virtual void WhenBackToGround() {}
    
    // Invoke this only when jumping starts.
    private void ResetAvoidGrounded() {
        avoidGrounded = false;
    }

    #endregion

    #region Speed

    public float MovementSpeed {
        get {
            return speedData.Speed;
        }
        private set {}
    }

    public float TargetSpeed {
        get {
            return horizInput * MovementSpeed;
        }
        private set {}
    }

    public virtual bool AbleToRun() {
        return isGrounded && !isRolling && !lockMovement;
    }

    //|=========================================================
    //| Using force to move instead of "transform.Translate"
    //| can make the character move more natural.
    //| This is a part of movement "FixedUpdate" process.
    //| Don't put this into "Update".
    //|=========================================================
    protected void ReachTargetSpeedByForce() {
        if (AbleToRun()) {
            float speedDiff = TargetSpeed - rbody.velocity.x;
            float movementForce = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, accelerationPow) * Mathf.Sign(speedDiff);
            rbody.AddForce(movementForce * Vector2.right);
        }
    }

    #endregion

    #region Jump

    //|=========================================================
    //| In many platformer games, heavier falling gravity would
    //| make the game look better. 
    //| This is a part of movement "Update" process.
    //| 
    //|=========================================================
    protected void ApplyFallingGravity() {
        if (rbody.velocity.y < 0) {
            rbody.gravityScale = gravityScale * fallingGravityMultiplier;
        } else {
            rbody.gravityScale = gravityScale;
        }
    }

    public virtual bool AbleToJump() {
        return isGrounded && !isRolling && !lockMovement;
    }

    //|=========================================================
    //| Use "Rigidbody2D.AddForce" to jump.
    //| Adding some horizontal force to reach "TargetSpeed" can
    //| make jumping interact with honrizontal input, making it
    //| more natural.
    //|=========================================================
    protected bool Jump() {
        if (AbleToJump()) {
            // Compute the honrizontal force
            float maxHorizForce = rbody.mass * (TargetSpeed - rbody.velocity.x);
            float horizForce = horizInput * jumpForce.x;
            if (Mathf.Abs(horizForce) > Mathf.Abs(maxHorizForce)) horizForce = maxHorizForce;

            // Apply force
            Vector2 force = new Vector2(horizForce, jumpForce.y);
            rbody.AddForce(force, ForceMode2D.Impulse);

            // Setup parameters for follow-up process
            isJumping = true;
            jumpCutApplied = false;

            // Avoid grounded to make animator work.
            avoidGrounded = true;
            Invoke(nameof(ResetAvoidGrounded), avoidGroundedTime);

            return true;
        } else {   
            return false;
        }
    }

    //|=========================================================
    //| To achieve dynamic jumping height, we need to reduce the
    //| velocity.y by half when jump key released.
    //| Using force to implement feels more natural. 
    //| 
    //|=========================================================
    protected bool CutoffJump() {
        if (isJumping && !jumpCutApplied && rbody.velocity.y > 0) {
            Vector2 force = rbody.mass * rbody.velocity.y * 0.5f * Vector2.down;
            rbody.AddForce(force, ForceMode2D.Impulse);
            jumpCutApplied = true;
            return true;
        } else {
            return false;
        }
    }

    protected void ResetJump() {
        isJumping = false;
        jumpCutApplied = false;
    }

    #endregion

    #region Roll

    public virtual bool AbleToRoll() {
        return !isRolling && !rollingIsInCD && !lockMovement;
    }

    //|=========================================================
    //| Add rolling force to rbody to gave the character instant 
    //| acceleration. If you don't want to roll too fast, 
    //| "Brake()" before "Roll()".
    //| 
    //|=========================================================
    public bool Roll() {
        if (AbleToRoll()) {
            rbody.AddForce(Mathf.Sign(transform.localScale.x) * rollingForce * Vector2.right, ForceMode2D.Impulse);
            isRolling = true;
            return true;
        } else {
            return false;
        }
    }

    private void StartInvinciblePeriod() {
        if (isRolling && health) {
            isInInvinciblePeriod = true;
            health.isInvincible  = true;
        }
    }

    private void FinishInvinciblePeriod() {
        if (isInInvinciblePeriod) {
            isInInvinciblePeriod = false;
            health.isInvincible  = false;
        }
    }

    private void FinishRolling() {
        if (!isRolling) return;
        FinishInvinciblePeriod();
        isRolling = false;
        rollingIsInCD = true;
        Invoke(nameof(FinishRollingCD), rollingCD);
    }

    public bool IsPlayingRollingAnimClip() {
        if (TryGetComponent<Animator>(out Animator anim)) {
            return anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == rollingClipName;
        }
        return false;
    }

    //|=========================================================
    //| An important part of movement "Update" process.
    //| End rolling when rolling animation finishes.
    //| 
    //| 
    //|=========================================================
    public void UpdateRolling() {
        if (isRolling) {
            if (!IsPlayingRollingAnimClip()) {
                FinishRolling();
                return;
            }
            if (isInInvinciblePeriod) health.isInvincible = true;
        }
    }

    private void FinishRollingCD() {
        rollingIsInCD = false;
    }

    #endregion
}
