using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroValueDisplayer : MonoBehaviour
{
    [Header ("Text")]
    [SerializeField] Text healthText;
    [SerializeField] Text speedText;
    [SerializeField] Text attackText;
    [SerializeField] Text protectionText;
    [SerializeField] Text cooldownText;

    public HeroBrain bindingHero {get; private set;} = null;

    private void FixedUpdate() {
        if (bindingHero) {
            healthText.text = $"{bindingHero.health.Hp}/{bindingHero.health.MaxHp}";
            speedText.text = $"{bindingHero.speedData.Speed}";
            attackText.text = $"{bindingHero.damageData.Damage}";

            cooldownText.text = $"x{bindingHero.cooldownSpeedMultiplier.Multiplier}";
        }
    }

    public void Bind(HeroBrain hero) {
        bindingHero = hero;
    }

    public void Unbind() {
        bindingHero = null;
    }
}
