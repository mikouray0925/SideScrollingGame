using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] int enemyNum;
    public UnityEvent onEnemyDie;
    public UnityEvent onAllEnemiesDie;


    private void OnEnemyKilled() {
        if (enemyNum > 0) {
            enemyNum -= 1;
            onEnemyDie.Invoke();
            if (enemyNum == 0) onAllEnemiesDie.Invoke();
        }
    }
}
