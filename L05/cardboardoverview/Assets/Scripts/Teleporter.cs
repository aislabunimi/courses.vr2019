using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public Transform player;
    public Transform target;
    public float gazeStayTime;

    public float teleporterRotSpeed = 20f;

    bool pointerInside = false;
    float gazeStartTime = 1.0f;

    void Update()
    {
        if (pointerInside && Time.time - gazeStartTime >= gazeStayTime)
        {
            Teleport();
        }
        // Make the teleporter rotate
        transform.Rotate(new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime) * teleporterRotSpeed);
    }

    void Teleport()
    {
        player.position = target.position;
    }

    public void OnPointerEnter()
    {
        pointerInside = true;
        gazeStartTime = Time.time;
    }

    public void OnPointerExit()
    {
        pointerInside = false;
    }
}
