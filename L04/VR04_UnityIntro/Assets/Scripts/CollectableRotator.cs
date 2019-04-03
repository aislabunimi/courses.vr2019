using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRotator : MonoBehaviour
{
    [Range(0f,360f)]
    public float rotationSpeed = 180f; // Degrees per second
    public Vector3 rotationVector = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        // We need to convert the rotation vector from world space to local space if we want
        // to interprete the rotation vector in local coordinates
        transform.Rotate(transform.InverseTransformVector(rotationVector.normalized) * rotationSpeed * Time.deltaTime);
    }
}
