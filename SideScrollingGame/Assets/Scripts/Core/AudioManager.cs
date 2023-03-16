using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField] public AudioSource musicPlayer;
    [SerializeField] public AudioSource uiPlayer;
    static float effectVolume = 1.0f;

    [Header ("Sliders")]
    [SerializeField] Slider slider_music;
    [SerializeField] Slider slider_effect;
    [SerializeField] Slider slider_UI;

    [Header ("Debug")]
    [SerializeField] float dVolume_music;
    [SerializeField] float dVolume_effect;
    [SerializeField] float dVolume_UI;

    public static AudioManager instance {get; private set;}

    private void Awake() {
        instance = this;
    }

    private void Update() {
        dVolume_music  = MusicVolume;
        dVolume_effect = EffectVolume;
        dVolume_UI     = UiVolume;
    }

    public void InitSliderValues() {
        slider_music.value  = MusicVolume;
        slider_effect.value = EffectVolume;
        slider_UI.value     = UiVolume;
    }

    public void UpdateVolumeBySliders() {
        MusicVolume  = slider_music.value;
        EffectVolume = slider_effect.value;
        UiVolume     = slider_UI.value;
    }

    public static float EffectVolume {
        get {
            return effectVolume;
        }
        set {
            effectVolume = Mathf.Clamp(value, 0f, 1f);
        }
    }

    public float MusicVolume {
        get {
            return musicPlayer.volume;
        }
        set {
            if (instance) {
                musicPlayer.volume = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }

    public float UiVolume {
        get {
            return uiPlayer.volume;
        }
        set {
            if (instance) {
                uiPlayer.volume = Mathf.Clamp(value, 0f, 1f);
            }
        }
    }
}
