using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangerAbility1 : HeroAbility1
{
    [Header ("Arrow rain")]
    [SerializeField] GameObject fallingArrowPrefab;
    [SerializeField] Transform arrowHolder;
    [SerializeField] Transform arrowLaunchPoint;
    [SerializeField] Transform localArrowStartPoint;
    [SerializeField] Transform arrowStartPoint;
    [SerializeField] float fallingRange;
    [SerializeField] float fallingArrowDamageMultiplier = 0.2f;
    [Range (1f, 50f)]
    [SerializeField] int arrowNum;
    [Range (0.01f, 1f)]
    [SerializeField] float minIntervalTime;
    [Range (0.01f, 1f)]
    [SerializeField] float maxIntervalTime;

    [Header ("Root wave")]
    [SerializeField] RangerRootWave rootWave;
    [SerializeField] float rootWaveDamageMultiplier = 1.2f;

    ObjPool<FallingSmallArrow> arrowPool;

    protected override void Awake() {
        base.Awake();
        arrowPool = new ObjPool<FallingSmallArrow>(fallingArrowPrefab, arrowHolder, 15);
    }
    
    public override bool AbleToAttack() {
        return 
        !isAttacking &&
        !attackCD.IsInCD && 
        movement.isGrounded && 
        !movement.isJumping && 
        !movement.isRolling;
    }

    public override void UnleashAbility1() {
        if (AbleToAttack()) {
            anim.SetTrigger("ability1");
            movement.movementLock.AddLock("ability1", 1.2f);
            movement.Brake();
        } 
    }
     
    private void StartArrowRain() {
        float heightDiff = localArrowStartPoint.position.y - arrowLaunchPoint.position.y;
        RaycastHit2D hit = Physics2D.Linecast(arrowLaunchPoint.position, arrowLaunchPoint.position + Vector3.up * heightDiff, GlobalSettings.obstacleLayers);
        if (hit.collider) {
            arrowStartPoint.position = new Vector2(localArrowStartPoint.position.x, hit.point.y - 0.1f);
        } else {
            arrowStartPoint.position = localArrowStartPoint.position;
        }
        StartCoroutine(ArrowRainCoroutine());

        if (rootWave) {
            Vector3 pos = rootWave.transform.position;
            pos.x = arrowStartPoint.position.x;
            rootWave.transform.position = pos;
            rootWave.damage = new Damage(this, damageData.Damage* rootWaveDamageMultiplier, Vector2.down);
        }
    }

    private IEnumerator ArrowRainCoroutine() {
        float y = arrowStartPoint.position.y;
        float halfFallingRange = fallingRange / 2f;
        float minX = arrowStartPoint.position.x - halfFallingRange;
        float maxX = arrowStartPoint.position.x + halfFallingRange;
        for (int i = 0; i < arrowNum; i++) {
            float x = Random.Range(minX, maxX);

            FallingSmallArrow fallingArrow = arrowPool.Get();
            fallingArrow.inPool = arrowPool;
            fallingArrow.transform.position = new Vector3(x, y, 0);
            fallingArrow.rb.velocity = Vector2.zero;
            fallingArrow.damage = new Damage(this, damageData.Damage * fallingArrowDamageMultiplier, Vector2.down);
            fallingArrow.rootWave = rootWave;
            fallingArrow.Launch();

            yield return new WaitForSeconds(Random.Range(minIntervalTime, maxIntervalTime));
        }
    }

    protected override void OnAttackFinish() {
        attackCD.StartCooldownCoroutine();
        movement.movementLock.RemoveLock("ability1");
    }

    private void OnDrawGizmosSelected() {
        if (!arrowStartPoint) return;
        float halfFallingRange = fallingRange / 2f;
        Gizmos.DrawLine(localArrowStartPoint.position, localArrowStartPoint.position + Vector3.right * halfFallingRange);
        Gizmos.DrawLine(localArrowStartPoint.position, localArrowStartPoint.position + Vector3.left  * halfFallingRange);
    }
}
