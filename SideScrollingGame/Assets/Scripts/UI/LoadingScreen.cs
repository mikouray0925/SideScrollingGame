using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : InterfaceUI
{
    [Header ("Slider")]
    public Slider progressSlider;

    public void ResetProgress() {
        progressSlider.value = 0f;
    }
}
