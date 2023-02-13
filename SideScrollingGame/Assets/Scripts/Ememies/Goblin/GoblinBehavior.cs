using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBehavior : MonoBehaviour
{
    [SerializeField] Sight sight;
    [SerializeField] Movement movement;

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
            movement.Brake();
            movement.horizInput = 0;
        }
    }

    private void MoveForward() {
        movement.horizInput = Mathf.Sign(transform.localScale.x);
    }
}
