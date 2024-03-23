using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private GameObject cam;

    [SerializeField] float parallaxEffect;

    float xPosition;
    float length;
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        xPosition = transform.position.x;

        length = GetComponent<SpriteRenderer>().size.x;  //Get length of bg.
    }

    void Update()
    {
        float distanceMoved = cam.transform.position.x * (1 - parallaxEffect);   //This is for endless bg.
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceMoved > xPosition + length)      //Makes endless bg.
            xPosition += length;
        else if (distanceMoved < xPosition - length)
            xPosition -= length;
    }
}
