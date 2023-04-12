using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // Start is called before the first frame update
    //Const value
    private Vector2 STARTING_POSITION = new Vector2(-41.42f, 4.2f); 
    private float ENDING_POSITION = 44.83f;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.x >= ENDING_POSITION)
        {
            transform.position = STARTING_POSITION;
        }
        else
        {
            transform.position = new Vector2(transform.position.x + 0.01f, transform.position.y);
        }
    }
}
