using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] float maxHealth;
    [SerializeField] float currentHealth;

    [Header ("Life")]
    [SerializeField] public int lifeRemain;
    [Range (0f, 1f)]
    [SerializeField] public float rebornPercentage;

    [Header ("other")]
    [SerializeField] public bool isInvincible;

    protected SpriteRenderer rend;

    #region HealthPoint

    public float MaxHp {
        get {
            return maxHealth;
        }
        private set {
            maxHealth = value;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        }
    }

    //|=======================================================
    //| The "value" will be clamped between 0 and "maxHealth".
    //| Use "TakeDamage" function instead of "Hp -= x" 
    //| while being attacked.
    //| 
    //|=======================================================
    public float Hp {
        get {
            return currentHealth;
        }
        private set {
            if (lifeRemain <= 0) return;

            value = Mathf.Clamp(value, 0f, maxHealth);
            if (value > currentHealth) {
                OnHealthIncrease();
            }
            if (value < currentHealth) {
                OnHealthDecrease();
            }  

            currentHealth = value;
            if (currentHealth == 0) {
                lifeRemain--;
                if (lifeRemain > 0) {
                    OnReborn();
                    currentHealth = maxHealth * rebornPercentage;
                } else {
                    OnLifeNumBecomeZero();
                }
            }
        }
    }

    protected virtual void OnHealthIncrease() {}
    protected virtual void OnHealthDecrease() {}

    #endregion 

    #region Life
    
    protected virtual void OnReborn() {}
    protected virtual void OnLifeNumBecomeZero() {}

    #endregion 

    #region Damage

    //|=======================================================
    //| Take damage by attack.
    //| If no life remains or "isInvincible" is true, 
    //| this function won't work.
    //| 
    //|=======================================================
    public void TakeDamage(Damage damageInfo) {
        if (lifeRemain <= 0 || isInvincible) return;  
        OnTakingDamage(damageInfo, out float finalDamageVal);
        Hp -= finalDamageVal;
    }

    protected void ProcessDamageDefault(Damage damageInfo) {
        if (TryGetComponent<Rigidbody2D>(out Rigidbody2D _rb)) {
            foreach (Damage.Force forceInfo in damageInfo.forces) {
                _rb.AddForce(forceInfo.force, forceInfo.mode);
            }
        }
    }

    protected virtual void OnTakingDamage(Damage damageInfo, out float finalDamageVal) {
        ProcessDamageDefault(damageInfo);
        finalDamageVal = damageInfo.damage;
    }

    #endregion

    #region Utility

    public void MakeSpriteFlash(Color color, float duraction, float delay = 0) {
        if (!rend) return;
        StartCoroutine(SpriteFlashCoroutine(color, duraction, delay));
    }

    public IEnumerator SpriteFlashCoroutine(Color color, float duraction, float delay) {
        yield return new WaitForSeconds(delay);
        // Color originalColor = rend.color;
        rend.color = color;
        yield return new WaitForSeconds(duraction);
        rend.color = Color.white;
    }

    #endregion
}
