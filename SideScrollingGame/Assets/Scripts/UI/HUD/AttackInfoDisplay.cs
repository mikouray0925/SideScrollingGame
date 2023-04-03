using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackInfoDisplay : MonoBehaviour
{
    [Header ("References")]
    public Image icon;
    [SerializeField] Image mask;
    [SerializeField] Text timerText;
    
    public Attack atk;

    private void LateUpdate() {
        if (atk != null) {
            mask.fillAmount = atk.attackCD.CooldownRemainingRatio;
            if (atk.attackCD.CooldownRemainingTime > 0) {
                timerText.text = Mathf.Ceil(atk.attackCD.CooldownRemainingTime).ToString();
            } else {
                timerText.text = "";
            }
        }
    }
}
