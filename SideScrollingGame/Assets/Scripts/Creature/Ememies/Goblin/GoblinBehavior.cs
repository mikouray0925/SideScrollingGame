using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBehavior : MonoBehaviour
{
    [SerializeField] Sight sight;

    [Header ("Movement")]
    [SerializeField] Movement movement;
    [SerializeField] PatrolPoint currentPatrolPoint;
    [SerializeField] float stopTime;
    [SerializeField] bool stopPatrolling;

    [Header ("Attack")]
    [SerializeField] GoblinNormalAttack normalAttack;
    [SerializeField] float normalAttackDistance;
    
    private void Update() {
        if (sight.CanSeeTarget(out Transform target, out float distance)) {
            if (distance <= normalAttackDistance) {
                movement.horizInput = 0;
                normalAttack.UnleashNormalAttack();
            }
            else {
                MoveForward();
            }
        }
        else {
            Patrol();
        }
        AvoidFalling();
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
        if (!movement.IsGrounded(0.5f * Mathf.Sign(transform.localScale.x) * Vector2.right)) {
            movement.Brake();
            movement.horizInput = 0;
        }
    }
}
