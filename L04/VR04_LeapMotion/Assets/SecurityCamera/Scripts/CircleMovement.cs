using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour {

    public float radius = 5f;
	
	// Update is called once per frame
	void Update () {

        this.transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.time) * radius, Mathf.Cos(Time.time) * radius);
	}
}
