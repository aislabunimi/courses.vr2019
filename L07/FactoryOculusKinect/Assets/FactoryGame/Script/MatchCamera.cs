using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchCamera : MonoBehaviour {

    float prevZPos;

	// Use this for initialization
	void Start () {
		prevZPos = Camera.main.transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        float cameraZPos = Camera.main.transform.position.z;
        float offset = cameraZPos - prevZPos;
        Vector3 newPos = transform.position;
        newPos.z += offset;
        transform.position = newPos;
        prevZPos = cameraZPos;
	}
}
