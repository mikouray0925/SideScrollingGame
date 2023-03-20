using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//|=======================================================
//| Multiple parts of one creature(chest, leg, ...)
//| may share one common <Health> component.
//| So I create this class. Mabe I should name this "IDamageable".
//|=======================================================
public class DamageablePart : MonoBehaviour
{
    [Header ("References")]
    public Health health;
    public Transform impactPointHolder;

    enum CenterType {
        CenterOfCollider,
        TransformOfThis,
        AnotherTransform
    }
    [Header ("Center")]
    [SerializeField] private CenterType centerType;
    [SerializeField] private Transform centerReplacement;

    //|=======================================================
    //| Try to get <DamageablePart> from each <Collider2D>
    //| in "colliders" and put them into a List.
    //| 
    //| 
    //|=======================================================
    public static List<DamageablePart> GetDamageableParts(Collider2D[] colliders) {
        List<DamageablePart> damageableList = new List<DamageablePart>();
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                if (damageable.enabled) damageableList.Add(damageable);
            }
        }
        return damageableList;
    }

    //|=======================================================
    //| Try to get <Health> from each <DamageablePart> in
    //| "damageableList" without repeat and put them into a 
    //| HashSet.
    //| 
    //|=======================================================
    public static HashSet<Health> GetHealthComponents(List<DamageablePart> damageableList) {
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (DamageablePart damageable in damageableList) {
            if (damageable.enabled) healthSet.Add(damageable.health);
        }
        return healthSet;
    }

    //|=======================================================
    //| Try to get <Health> from each <Collider2D> in
    //| "colliders" without repeat and put them into a HashSet.
    //| 
    //| 
    //|=======================================================
    public static HashSet<Health> GetHealthComponents(Collider2D[] colliders) {
        return GetHealthComponents(GetDamageableParts(colliders));

        /* Maybe this is better
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                if (damageable.enabled) healthSet.Add(damageable.health);
            }
        }
        return healthSet;
        */
    }

    //|=======================================================
    //| The center position of this <DamageablePart>.
    //| get: Get the center position with specified center type.
    //| set: Set the transform.position by the relative position 
    //|      of Center.
    //|=======================================================
    public Vector2 Center {
        get {
            switch (centerType) {
            case CenterType.CenterOfCollider:
                return GetComponent<Collider2D>().bounds.center;
            
            case CenterType.AnotherTransform:
                return centerReplacement.position;
            
            case CenterType.TransformOfThis:
            default:
                return transform.position;
            }
        }
        set {
            Vector3 offset = transform.position - Vec2Util.ToVec3(Center);
            // transform.position = Center + offset
            Vector3 newCenterPos = value;
            transform.position = newCenterPos + offset;
        }
    }
}
