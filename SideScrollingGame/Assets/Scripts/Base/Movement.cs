using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header ("Inputs")]
    [SerializeField] public float horizInput;
    [SerializeField] public float vertiInput;
    
    [Header ("Movement parameters")]
    [SerializeField] private   float movementSpeed;
    [SerializeField] private   float speedMultiplier;
    [SerializeField] protected float acceleration;
    [SerializeField] protected float accelerationPow;

    #region ComponentRef
    protected CapsuleCollider2D capCollider;
    protected Rigidbody2D       rbody;
    protected Animator          anim;
    protected SpriteRenderer    rend;
    #endregion

    public float MovementSpeed {
        get {
            return movementSpeed * speedMultiplier;
        }
        private set {}
    }

    public float TargetSpeed {
        get {
            return horizInput * movementSpeed;
        }
        private set {}
    }

    protected void GrabBasicComponent()  {
        capCollider = GetComponent<CapsuleCollider2D>();
        rbody       = GetComponent<Rigidbody2D>();
        anim        = GetComponent<Animator>();
        rend        = GetComponent<SpriteRenderer>();
    }

    public void GrabInputsByInputSystem()  {
        horizInput = Input.GetAxis("Horizontal");
        vertiInput = Input.GetAxis("Vertical");
    }

    public void AddTempSpeedMultiplier(float multiplier, float duraction) {
        StartCoroutine(SpeedMultiplierCoroutine(multiplier, duraction));
    }

    private IEnumerator SpeedMultiplierCoroutine(float multiplier, float duraction) {
        speedMultiplier *= multiplier;
        yield return new WaitForSeconds(duraction);
        speedMultiplier /= multiplier;
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

    protected void ReachTargetSpeedByForce() {
        float speedDiff = TargetSpeed - rbody.velocity.x;
        float movementForce = Mathf.Pow(speedDiff * acceleration, accelerationPow) * Mathf.Sign(speedDiff);
        rbody.AddForce(movementForce * Vector2.right);
    }

}
