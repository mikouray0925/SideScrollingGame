using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class GobKingBehavior : MonoBehaviour
{
    [SerializeField] Sight sight;

    [Header ("Movement")]
    [SerializeField] Movement movement;
    [SerializeField] PatrolPoint currentPatrolPoint;
    [SerializeField] float stopTime;
    [SerializeField] bool stopPatrolling;

    [Header ("Attack")]
    [MinMaxSlider (0f, 20f)]
    [SerializeField] Vector2 dashAttackRange;
    [SerializeField] GobKingDashAttack dashAttack;
    [SerializeField] float hornAttackDistance;
    [SerializeField] GobKingHornAttack hornAttack;
    
    
    private void Update() {
        if (!dashAttack.isAttacking && !hornAttack.isAttacking) {
            if (sight.CanSeeTarget(out Transform target, out float distance)) {
                if (distance <= hornAttackDistance) {
                    if (hornAttack.AbleToAttack()) {
                        hornAttack.UnleashHornAttack();
                    } else {
                        movement.horizInput = 0;
                        movement.Brake();
                    }
                }
                else if (distance >= dashAttackRange.x && distance <= dashAttackRange.y) {
                    if (!dashAttack.attackCD.IsInCD) {
                        dashAttack.UnleashDashAttack();
                    } else MoveForward();
                } else MoveForward();
            }
            else {
                Patrol();
            }
        } 
        
        //AvoidFalling();
    }

    private void MoveForward() {
        movement.horizInput = Mathf.Sign(transform.localScale.x);
    }

    private void Patrol() {
        if (stopPatrolling) {
            movement.horizInput = 0;
            return;
        }

        Vector2 groundPoint = movement.GroundCheckboxCenter;
        if (currentPatrolPoint.HowFar(groundPoint) < 0.2f) {
            movement.Brake();
            movement.horizInput = 0;
            stopPatrolling = true;
            Invoke(nameof(GoNextPatrolPoint), stopTime);
        }

        movement.horizInput = Mathf.Sign(currentPatrolPoint.position.x - movement.GroundCheckboxCenter.x);
    }

    public void GoNextPatrolPoint() {
        stopPatrolling = false;
        currentPatrolPoint = currentPatrolPoint.nextPoint;
    }

    private void AvoidFalling() {
        if (!movement.IsGrounded(0.2f * Mathf.Sign(transform.localScale.x) * Vector2.right)) {
            movement.Brake();
            movement.horizInput = 0;
            dashAttack.FinishAttackAnim();
        }
    }
}
