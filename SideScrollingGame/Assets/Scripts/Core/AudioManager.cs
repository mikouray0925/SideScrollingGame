using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header ("Components")]
    [SerializeField] public AudioSource musicPlayer;
    [SerializeField] public AudioSource uiPlayer;

    static float effectVolume = 1.0f;

    public static AudioManager instance {get; private set;}

    private void Awake() {
        instance = this;
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
