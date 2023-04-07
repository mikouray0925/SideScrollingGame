using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : InterfaceUI
{
    [Header ("Slider")]
    [SerializeField] private Slider progressSlider;

    [Header ("values")]
    public float progress;
    public float dampingTime;

    private float dampingVelocity;

    private void Update() {
        if (progressSlider.value < progress) {
            progressSlider.value = Mathf.SmoothDamp(progressSlider.value, progress, ref dampingVelocity, dampingTime);
        }
    }

    public bool SliderReachProgress {
        get {
            return Mathf.Abs(progress - progressSlider.value) < 0.01f;
        }
        private set {}
    }

    public void ResetProgress() {
        progressSlider.value = 0f;
        progress = 0f;
        dampingVelocity = 0f;
    }
}
