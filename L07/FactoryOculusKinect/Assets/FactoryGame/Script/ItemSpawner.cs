using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {
	
	public LayerMask collisionMask;
	public GameObject item;
	public float itemsPerSecond = 1.0f;

	bool autoSpawnEnabled = true;


	Vector3 halfCube = new Vector3(.5f,.5f,.5f);

	// Use this for initialization
	void Start () {
		if(autoSpawnEnabled){
			Invoke("SpawnItem", 1.0f / itemsPerSecond);
		}
	}

	public void SpawnItem(){
		SpawnItem(null);
	}

	public void SpawnItem(GameObject prefab = null)
	{
		RaycastHit hit;
		Collider[] colliders = Physics.OverlapBox(transform.position, halfCube, transform.rotation, collisionMask);

		// Spawn only if there is space for the cube
		if(colliders.Length == 0){
			// Instantiate prefab, set its position and parent
			GameObject spawned = Instantiate<GameObject>(prefab == null ? item : prefab);
			spawned.transform.position = transform.position;
			spawned.transform.SetParent(transform);
		}

		if(autoSpawnEnabled){
			Invoke("SpawnItem", 1.0f / itemsPerSecond);
		}
	}

	public void EnableAutoSpawn(){
		autoSpawnEnabled = true;
		Invoke("SpawnItem", 1.0f / itemsPerSecond);
	}

	public void DisableAutoSpawn(){
		autoSpawnEnabled = false;
	}

}
