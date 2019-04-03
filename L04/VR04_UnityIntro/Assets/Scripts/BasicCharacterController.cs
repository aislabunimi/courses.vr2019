using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCharacterController : MonoBehaviour
{
    // Public fields are usually exposed in the unity inspector
    // Unless the HideInInspector directive is used
    public float movementSpeed = 5f;        // Units per second

    // Some directives such as the "Range" one allow you to change the widget used to control
    // the variable in the Unity Inspector
    [Range(1f,360f)]
    public float rotationSpeed = 90f;       // Angles per second

    // Private fields are hidden (but visible in debug mode)
    private Vector3 movementVector;
    private Vector3 rotationVector;

    // Start is called before the first frame update
    void Start()
    {
        movementVector = new Vector3();
        rotationVector = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        movementVector.z = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        rotationVector.y = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        transform.Translate(movementVector);
        transform.Rotate(rotationVector);
    }
}
