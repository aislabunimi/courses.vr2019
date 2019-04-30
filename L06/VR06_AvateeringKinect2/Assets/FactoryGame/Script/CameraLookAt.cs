using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

    public Transform target;

    public bool isTracking;
    public bool isBlinking;
	
	// Update is called once per frame
	void Update () {
		if(target){
	        // Rotate the camera every frame so it keeps looking at the target
	        if (isTracking)
	        {
	           Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position, new Vector3(0, 1, 0));

	            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
	        }

	        // Blink to implement...
		}
    }
}
