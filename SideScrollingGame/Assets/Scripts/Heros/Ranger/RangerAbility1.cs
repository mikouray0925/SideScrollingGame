using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAbility1 : Attack
{
    [Header ("Arrow rain")]
    [SerializeField] ProjectilePool fallingArrowPool;
    [SerializeField] Transform localArrowStartPoint;
    [SerializeField] Transform arrowStartPoint;
    [SerializeField] float fallingRange;
    [Range (1f, 50f)]
    [SerializeField] int arrowNum;
    [Range (0.01f, 1f)]
    [SerializeField] float minIntervalTime;
    [Range (0.01f, 1f)]
    [SerializeField] float maxIntervalTime;

    [Header ("Root wave")]
    [SerializeField] RangerRootWave rootWave;

    Movement movement;

    private void Awake() {
        movement = GetComponent<Movement>();
        anim     = GetComponent<Animator>();
    }

    private void Update() {
        if (isAttacking && !IsPlayingAttackAnimClip()) FinishAbility1();
        if (Input.GetButtonDown("Fire2")) UnleashAbility1();
    }

    public override bool AbleToAttack() {
        return 
        !isAttacking &&
        !attackCD.IsInCD && 
        movement.isGrounded && 
        !movement.isJumping && 
        !movement.isRolling;
    }

    public bool UnleashAbility1() {
        if (AbleToAttack()) {
            anim.SetTrigger("ability1");
            movement.LockMovementForSeconds(1.2f);
            movement.Brake();
            isAttacking = true;
        } 
        return isAttacking;
    }
     
    private void StartArrowRain() {
        arrowStartPoint.position = localArrowStartPoint.position;
        if (rootWave) {
            Vector3 pos = rootWave.transform.position;
            pos.x = arrowStartPoint.position.x;
            rootWave.transform.position = pos;
            rootWave.damage = damageData.Damage * 1.2f;
        }
        StartCoroutine(ArrowRainCoroutine());
    }

    private IEnumerator ArrowRainCoroutine() {
        float y = arrowStartPoint.position.y;
        float halfFallingRange = fallingRange / 2f;
        float minX = arrowStartPoint.position.x - halfFallingRange;
        float maxX = arrowStartPoint.position.x + halfFallingRange;
        for (int i = 0; i < arrowNum; i++) {
            float x = Random.Range(minX, maxX);

            FallingSmallArrow fallingArrow = fallingArrowPool.GetInactiveProjectile() as FallingSmallArrow;
            fallingArrow.Activate();
            fallingArrow.transform.position = new Vector3(x, y, 0);
            fallingArrow.rb.velocity = Vector2.zero;
            fallingArrow.damage = damageData.Damage * 0.1f;
            fallingArrow.rootWave = rootWave;
            fallingArrow.Launch();

            yield return new WaitForSeconds(Random.Range(minIntervalTime, maxIntervalTime));
        }
    }

    private void FinishAbility1() {
        attackCD.StartCooldownCoroutine();
        movement.UnlockMovement();
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        if (!arrowStartPoint) return;
        float halfFallingRange = fallingRange / 2f;
        Gizmos.DrawLine(localArrowStartPoint.position, localArrowStartPoint.position + Vector3.right * halfFallingRange);
        Gizmos.DrawLine(localArrowStartPoint.position, localArrowStartPoint.position + Vector3.left  * halfFallingRange);
    }
}
