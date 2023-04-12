using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectModify : MonoBehaviour
{
    private Camera cam;
    [SerializeField] public int width = 1920;
    [SerializeField] public int height = 1080;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.aspect = width/height;
    }

    void Update()
    {
        cam.aspect = width/height;
    }


}
