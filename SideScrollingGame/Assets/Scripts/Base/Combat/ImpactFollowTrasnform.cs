using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactFollowTrasnform : MonoBehaviour
{
    [SerializeField] float lifespan;
    public bool followPosition;
    public bool followRotation;
    public bool followFlipping;
    
    Transform followingTransform;
    Vector3 lastFollowingScale;

    void Awake() {
        Invoke(nameof(DestroyThisAndFollowing), lifespan);
    }

    void Update() {
        if (followingTransform) {
            if (followPosition) transform.position = followingTransform.position;
            if (followRotation) transform.rotation = followingTransform.rotation;
            if (followFlipping && (Mathf.Sign(followingTransform.lossyScale.x) * Mathf.Sign(lastFollowingScale.x) < 0)) {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y , transform.localScale.z);
                lastFollowingScale = followingTransform.lossyScale;
            } 
        }    
    }

    public void Follow(Transform _followingTransform) {
        followingTransform = _followingTransform;
        lastFollowingScale = followingTransform.lossyScale;
    }

    public void DestroyThisAndFollowing() {
        Destroy(gameObject);
        if (followingTransform) Destroy(followingTransform.gameObject);
    }
}
