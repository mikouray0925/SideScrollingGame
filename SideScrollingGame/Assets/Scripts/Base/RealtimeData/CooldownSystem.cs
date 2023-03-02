using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownSystem : MonoBehaviour
{
    [SerializeField] private float debugClock;
    [SerializeField] private float cooldownTime;
    [SerializeField] public CombinedMultiplier cdSpeedMultiplier;
    [Range (0.05f, 1f)]
    [SerializeField] private float cooldownUpdatePeriod = 0.1f;
    private bool isInCD = false;
    private float cdRemainingTime;

    private void Update() {
        debugClock = CooldownRemainingTime;
    }

    public bool IsInCD {
        get {
            return isInCD;
        }
        private set {}
    }

    public float CooldownTime {
        get {
            if (cdSpeedMultiplier.Multiplier > 0) {
                return cooldownTime / cdSpeedMultiplier.Multiplier;
            } else {
                print("cdSpeedMultiplier is zero.");
                return Mathf.Infinity;
            }
        }
        private set {}
    }

    public float CooldownRemainingTime {
        get {
            if (isInCD) {
                if (cdSpeedMultiplier.Multiplier > 0) {
                    return cdRemainingTime / cdSpeedMultiplier.Multiplier;
                } else {
                    print("cdSpeedMultiplier is zero.");
                    return Mathf.Infinity;
                }
            }
            else {
                return 0;
            }
        }
        private set {}
    }

    public float CooldownRemainingProp {
        get {
            return cdRemainingTime / cooldownTime;
        }
        private set {}
    }

    public void StartCooldownCoroutineAfterSeconds(float delay) {
        Invoke(nameof(StartCooldownCoroutine), delay);
    }

    public void StartCooldownCoroutine() {
        if (isInCD) {
            print("CooldownCoroutine has started, this one will be cancel.");
        } else {
            StartCoroutine(CooldownCoroutine());
        }   
    }

    private IEnumerator CooldownCoroutine() {
        isInCD = true;
        cdRemainingTime = cooldownTime;
        float lastCheckTime = Time.time;
        while(cdRemainingTime > 0) {
            yield return new WaitForSeconds(cooldownUpdatePeriod);
            cdRemainingTime -= (Time.time - lastCheckTime) * cdSpeedMultiplier.Multiplier;
            lastCheckTime = Time.time;
        }
        isInCD = false;
    }
}
