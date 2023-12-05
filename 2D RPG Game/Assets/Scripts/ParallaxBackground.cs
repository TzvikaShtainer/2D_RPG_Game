using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private GameObject cam;
    
    [SerializeField]private float parallaxEffect;
    
    private float xPosition;
    private float length;

    private void Start()
    {
        cam = GameObject.Find("Main Camera");
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    private void Update()
    {
        float distanceToMove = cam.transform.position.x * parallaxEffect;
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);
        
        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceToMove > xPosition + length)
            xPosition = xPosition + length;
        else if (distanceToMove < xPosition - length)
            xPosition = xPosition - length;
    }
}
