using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using GVR;


// The target variable shows up as a property in the inspector.
// Drag another object onto it to make the camera look at it.

public class Target : MonoBehaviour {

    public Transform player;
    public float gazeStayTime = 1.0f;
    public bool useController = false;


    float gazeStartTime = 0f;
    bool pointerInside = false;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        // Use controller
        if (useController)
        {
            if (Input.GetButton("Fire1"))
            {
                OnGazeStay();
                pointerInside = false;
            }
        }
        // Handle Gaze On Target
        else
        {
            if (pointerInside && Time.time - gazeStartTime >= gazeStayTime)
            {
                OnGazeStay();
                pointerInside = false;
            }
        }

        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(player);
    }

    public void OnGazeStay()
    {
        if (pointerInside)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

    // Called everytime user look at target
    public void OnPointerEnter()
    {
        pointerInside = true;
        gazeStartTime = Time.time;
    }

    // Called everytime user stop looking at target
    public void OnPointerExit()
    {
        pointerInside = false;
    }

}
