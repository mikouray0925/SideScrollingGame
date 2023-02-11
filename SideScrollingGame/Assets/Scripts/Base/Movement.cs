using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header ("Inputs")]
    [SerializeField] public  float horizInput;
    [SerializeField] public  float vertiInput;
    [SerializeField] private bool lockMovement;

    [Header ("Ground check")]
    [SerializeField] private   Vector2 groundCheckboxOffset;
    [SerializeField] private   Vector2 groundCheckboxSize;
    [SerializeField] protected bool drawGroundCheckbox;
    [SerializeField] public    bool isGrounded {get; private set;}
    [SerializeField] private   bool avoidGrounded;

    [Header ("Movement parameters")]
    [SerializeField] private   float movementSpeed;
    [SerializeField] private   float speedMultiplier;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float accelerationPow;

    [Header ("Jump parameters")]
    [SerializeField] protected Vector2 jumpForce;
    [SerializeField] protected float gravityScale;
    [SerializeField] protected float fallingGravityMultiplier;
    [SerializeField] public    bool  isJumping {get; private set;}
    [SerializeField] private   bool  jumpCutApplied;
    [SerializeField] private   float avoidGroundedTime;

    [Header ("Rolling parameters")]
    [SerializeField] protected float rollingForce;
    [SerializeField] public    bool  isRolling {get; private set;}
    [SerializeField] protected float rollingCD;
    [SerializeField] private   bool  rollingIsInCD;

    #region ComponentRef
    protected Rigidbody2D       rbody;
    
    #endregion

    #region Basic

    public void DefaultUpdate() {
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

    protected void FlipWithHorizInput() {
        if (Mathf.Abs(horizInput) > 0 && Mathf.Sign(horizInput) != Mathf.Sign(transform.localScale.x)) {
            transform.localScale = new Vector3(
                -transform.localScale.x, 
                 transform.localScale.y, 
                 transform.localScale.z
            );
        }
    }

    public void LockMovementForSeconds(float duration) {
        lockMovement = true;
        Invoke(nameof(UnlockMovement), duration);
    }

    public void UnlockMovement() {
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

    protected bool CheckGround() {
        bool oldGroundedVal = isGrounded;
        if (avoidGrounded) {
            isGrounded = false;
        } else {
            RaycastHit2D hit = Physics2D.BoxCast(GroundCheckboxCenter, groundCheckboxSize, 0, Vector2.down, 0.1f, GameManager.groundLayers);
            isGrounded = (hit.collider != null && Vector2.Dot(hit.normal, Vector2.up) > 0);
        }
        if (!oldGroundedVal && isGrounded) BackToGround();
        return isGrounded;
    }

    protected virtual void BackToGround() {
        if (isJumping) ResetJump();
    }

    private void ResetAvoidGrounded() {
        avoidGrounded = false;
    }

    #endregion

    #region Speed

    public float MovementSpeed {
        get {
            return movementSpeed * speedMultiplier;
        }
        private set {}
    }

    public float TargetSpeed {
        get {
            return horizInput * MovementSpeed;
        }
        private set {}
    }

    public void AddTempSpeedMultiplier(float multiplier, float duration) {
        StartCoroutine(SpeedMultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedMultiplierCoroutine(float multiplier, float duration) {
        speedMultiplier *= multiplier;
        yield return new WaitForSeconds(duration);
        speedMultiplier /= multiplier;
    }

    public bool AbleToRun() {
        return  isGrounded && !isRolling && !lockMovement;
    }

    protected void ReachTargetSpeedByForce() {
        if (AbleToRun()) {
            float speedDiff = TargetSpeed - rbody.velocity.x;
            float movementForce = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, accelerationPow) * Mathf.Sign(speedDiff);
            rbody.AddForce(movementForce * Vector2.right);
        }
    }

    #endregion

    #region Jump

    protected void ApplyFallingGravity() {
        if (rbody.velocity.y > 0) {
            rbody.gravityScale = gravityScale * fallingGravityMultiplier;
        } else {
            rbody.gravityScale = gravityScale;
        }
    }

    protected bool AbleToJump() {
        return isGrounded && !isRolling && !lockMovement;
    }

    protected bool Jump() {
        if (AbleToJump()) {
            float maxHorizForce = rbody.mass * (TargetSpeed - rbody.velocity.x);
            float horizForce = horizInput * jumpForce.x;
            if (Mathf.Abs(horizForce) > Mathf.Abs(maxHorizForce)) horizForce = maxHorizForce;
            Vector2 force = new Vector2(horizForce, jumpForce.y);
            rbody.AddForce(force, ForceMode2D.Impulse);
            isJumping = true;
            jumpCutApplied = false;
            avoidGrounded = true;
            Invoke(nameof(ResetAvoidGrounded), avoidGroundedTime);
            return true;
        } else {   
            return false;
        }
    }

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

    public bool AbleToRoll() {
        return !isRolling && !rollingIsInCD && !lockMovement;
    }

    public bool Roll() {
        if (AbleToRoll()) {
            rbody.AddForce(Mathf.Sign(transform.localScale.x) * rollingForce * Vector2.right, ForceMode2D.Impulse);
            isRolling = true;
            return true;
        } else {
            return false;
        }
    }

    public void StopRolling() {
        if (isRolling) {
            isRolling = false;
            rollingIsInCD = true;
            Invoke(nameof(ResetRollingIsInCD), rollingCD);
        }
    }

    private void ResetRollingIsInCD() {
        rollingIsInCD = false;
    }

    #endregion
}
