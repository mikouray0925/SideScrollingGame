using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBrain : MonoBehaviour
{
    [Header ("References")]
    public HeroHealth health;
    public HeroMovement movement;
    public HeroNormalAttack normalAttack;
    public HeroAbility1 ability1;
    public HeroAbility2 ability2;
}
