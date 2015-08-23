using System;
using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

    //TODO figure out why does localposition.y moves when scrolling


    private float distance = -10f;
    private float height = 2f;
    private const float minCamDistance = 0f;
    private const float maxCamDistance = -20f;

    

    public float speed;

	// Use this for initialization
	void Start ()
	{
	    
	}
	
	// Update is called once per frame
	void Update () {
	
       
        CameraMovement();

	}


    void CameraMovement()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (transform.localPosition.z <= minCamDistance)
            {
                transform.Translate(0, 0, speed*Input.GetAxis("Mouse ScrollWheel"));
            }
            else
            {
                transform.Translate(0,0,minCamDistance);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (transform.localPosition.z >= maxCamDistance)
            {
                transform.Translate(0, 0, speed * Input.GetAxis("Mouse ScrollWheel"));
            }
            else
            {
                transform.Translate(0, 0, maxCamDistance);
            }
        }


    }
}
