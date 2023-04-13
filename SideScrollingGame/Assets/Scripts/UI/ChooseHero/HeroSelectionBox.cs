using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectionBox : MonoBehaviour
{
    [SerializeField] float unselectedScale = 1.0f;
    [SerializeField] float selectedScale = 1.2f;
    [SerializeField] float scalingTime = 0.2f;
    [SerializeField] HeroSelectionBoxRadio radio;
    [SerializeField] GameObject heroPrefab;

    float currentScalingSpeed;
    bool isSelected = false;

    private void Update() {
        float currentScale = transform.localScale.x;
        if ( isSelected && currentScale < selectedScale) {
            float newScale = Mathf.SmoothDamp(currentScale, selectedScale, ref currentScalingSpeed, scalingTime);
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }
        if (!isSelected && currentScale > unselectedScale) {
            float newScale = Mathf.SmoothDamp(currentScale, unselectedScale, ref currentScalingSpeed, scalingTime);
            transform.localScale = new Vector3(newScale, newScale, 1f);
        }
    }

    public void TriggerSelect() {
        if (radio) radio.UnselectOthers(this);
        if (isSelected) {
            if (AppManager.instance && AppManager.instance.localPlayer) {
                GameObject heroObj = Instantiate(heroPrefab);
                HeroBrain hero = heroObj.GetComponent<HeroBrain>();
                AppManager.instance.LocalHero = hero;
                AppManager.instance.localPlayer.playerData = new PlayerData();
                AppManager.instance.PlayGameLevel("Level1-1");
            } else {
                if (!AppManager.instance) 
                    Debug.LogError("The instance of AppManager is not set.");
                if (!AppManager.instance.localPlayer)
                    Debug.LogError("Some instance of AppManager.localPlayer is not set.");
            }
        }
        else {
            isSelected = true;
            currentScalingSpeed = 0;
        }
    }

    public void ResetSelect() {
        isSelected = false;
        currentScalingSpeed = 0;
    }
}
