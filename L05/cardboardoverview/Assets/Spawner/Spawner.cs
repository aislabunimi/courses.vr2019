using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public float radius;
    public float minHeight;
    public float maxHeight;

    public GameObject target;

    // Use this for initialization
    void Start () {
        InvokeRepeating("SpawnTarget", 1.0f,Random.Range(3.0f,5.0f));
	}

    // Spawn a target in the radius range
    public void SpawnTarget()
    {
        if (!target.GetComponent<Renderer>().enabled)
        {
            target.GetComponent<Renderer>().enabled = true;
            target.GetComponent<Collider>().enabled = true;
        }
        else
            GetComponent<AudioSource>().Play();

        float xPos = Random.Range(-1.0f, 1.0f) * radius;
        float zPos = Random.Range(-1.0f, 1.0f) * radius;
        float yPos = Random.Range(minHeight, maxHeight);

        Vector3 spawnPos = new Vector3(xPos, yPos, zPos);

        target.transform.position = spawnPos;
    }
}
