using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public Transform attacker;
    public Attack attack;
    public float damage;
    public Vector2 mainDirection;

    public class Force {
        public Vector3 force;
        public ForceMode2D mode;
    }
    public List<Force> forces;

    public Damage() {}

    public Damage(Attack _attack, float _damage, Vector2 _mainDirection) {
        attack = _attack;
        attacker = attack.transform;
        damage = _damage;
        mainDirection = _mainDirection;
    }
}
