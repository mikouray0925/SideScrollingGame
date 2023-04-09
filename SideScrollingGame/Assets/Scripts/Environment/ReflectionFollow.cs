using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionFollow : MonoBehaviour
{
    [SerializeField] public CameraFollow ReflectCam;

    void LateUpdate()
    {
        if (ReflectCam)
        {
            transform.position = new Vector3(ReflectCam.transform.position.x, transform.position.y, transform.position.z);
        }
    }
    
}
