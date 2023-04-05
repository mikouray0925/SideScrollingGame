using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;
    [SerializeField] Slider valueSlider;

    [Header ("Damping")]
    [SerializeField] Slider dampSlider;
    [SerializeField] float dampingTime;
    bool isDamping;
    float dampingVelocity;


    void Update() {
        if (health) {
            float percentage = health.Hp / health.MaxHp;
            if (valueSlider.value != percentage) {
                valueSlider.value = percentage;
            }

            if (dampSlider) {
                if (dampSlider.value != valueSlider.value && !isDamping) {
                    isDamping = true;
                    dampingVelocity = 0;
                }
                if (isDamping) {
                    dampSlider.value = Mathf.SmoothDamp(dampSlider.value, valueSlider.value, ref dampingVelocity, dampingTime);
                    if (dampSlider.value == valueSlider.value) isDamping = false;
                }
            }
        }
    }
}
