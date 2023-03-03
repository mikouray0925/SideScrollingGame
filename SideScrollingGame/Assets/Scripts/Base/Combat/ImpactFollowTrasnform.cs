using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFollowTrasnform : MonoBehaviour
{
    [SerializeField] float lifespan;
    public Transform followingTransform;
    public bool followPosition;
    public bool followRotation;

    void Awake() {
        Invoke(nameof(DestroyThisAndFollowing), lifespan);
    }

    void Update() {
        if (followingTransform) {
            if (followPosition) transform.position = followingTransform.position;
            if (followRotation) transform.rotation = followingTransform.rotation;
        }    
    }

    void DestroyThisAndFollowing() {
        Destroy(gameObject);
        if (followingTransform) Destroy(followingTransform.gameObject);
    }
}
