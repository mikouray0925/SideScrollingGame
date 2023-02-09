using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header ("Inputs")]
    [SerializeField] public float horizInput;
    [SerializeField] public float vertiInput;

    [Header ("Ground check")]
    [SerializeField] protected Vector2 groundCheckboxOffset;
    [SerializeField] protected Vector2 groundCheckboxSize;
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
    [SerializeField] protected bool  isJumping;
    [SerializeField] protected bool  jumpCutApplied;
    [SerializeField] private   float avoidGroundedTime;

    #region ComponentRef
    protected Rigidbody2D       rbody;
    
    #endregion

    #region Basic

    protected void GrabNecessaryComponent()  {
        rbody = GetComponent<Rigidbody2D>();
    }

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

    private void OnDrawGizmosSelected() {
        if (drawGroundCheckbox) Gizmos.DrawWireCube(GroundCheckboxCenter, groundCheckboxSize);
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

    public void AddTempSpeedMultiplier(float multiplier, float duraction) {
        StartCoroutine(SpeedMultiplierCoroutine(multiplier, duraction));
    }

    private IEnumerator SpeedMultiplierCoroutine(float multiplier, float duraction) {
        speedMultiplier *= multiplier;
        yield return new WaitForSeconds(duraction);
        speedMultiplier /= multiplier;
    }

    protected void ReachTargetSpeedByForce() {
        if (!isGrounded) return;
        float speedDiff = TargetSpeed - rbody.velocity.x;
        float movementForce = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, accelerationPow) * Mathf.Sign(speedDiff);
        rbody.AddForce(movementForce * Vector2.right);
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
        return isGrounded;
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
}
