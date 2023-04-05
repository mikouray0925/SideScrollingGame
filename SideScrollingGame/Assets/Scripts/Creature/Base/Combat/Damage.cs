using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//|=========================================================
//| An interface of damage. Maybe I should name this "IDamage".
//| Attack process:
//| <Attack>new                    ↱ <Health>TakeDamage
//|           ↳ <Damage> -> <DamageablePart>
//|=========================================================
public class Damage
{
    public Transform attacker;
    public Attack attack;
    public float damage;
    public Vector2 mainDirection;

    public class Force {
        public Vector3 force;
        public ForceMode2D mode;

        public Force(Vector3 _force, ForceMode2D _mode) {
            force = _force;
            mode  = _mode;
        }
    }
    public List<Force> forces = new List<Force>();

    public Damage() {}

    public Damage(Attack _attack, float _damage, Vector2 _mainDirection) {
        attack = _attack;
        attacker = attack.transform;
        damage = _damage;
        mainDirection = _mainDirection;
    }
}
