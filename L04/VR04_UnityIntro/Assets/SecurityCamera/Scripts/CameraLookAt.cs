using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour {

    // The transform of the object that we are going to rotate
    public Transform transformToRotate;

    // The transform to follow
    public Transform target;

    public float speedMov = 2f;
	
	// Update is called once per frame
	void Update () {

        // Rotate the camera every frame so it keeps looking at the target
        if (target)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transformToRotate.position, new Vector3(0, 1, 0));
            transformToRotate.rotation = Quaternion.Slerp(transformToRotate.rotation, targetRotation, Time.deltaTime * speedMov);
        }

    }
}
