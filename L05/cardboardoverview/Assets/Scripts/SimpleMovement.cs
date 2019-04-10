using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    public float movementForce = 10f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Using camera forward vector to move in the direction we are looking at
        Vector3 translationVector = Camera.main.transform.forward;
        // Since we don't want our character to be able to fly we ignore the y component
        translationVector.y = 0f;
        // Translation happen here...
        //transform.Translate(translationVector * Input.GetAxis("Vertical") * Time.deltaTime * maxSpeed);
        rb.AddForce(translationVector * Input.GetAxis("Vertical") * movementForce, ForceMode.Force);
    }
}