using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour {

    public Transform destination;
    public NavMeshAgent player;

    bool isMoving = false;

	// Use this for initialization
	void Start () {

        // Cache agent component and destination
        player = GetComponent<NavMeshAgent>();
    }

    public void ToggleMovement()
    {
        if (isMoving) PauseMovement();
        else GotoExit();
    }
	
    void PauseMovement()
    {
        player.destination = transform.position;
        isMoving = false;
    }

	void GotoExit()
    {
        if (Vector3.Distance(destination.position, this.transform.position) > 1.0f)
        {
            player.destination = destination.position;
            isMoving = true;
        }
    }
}
