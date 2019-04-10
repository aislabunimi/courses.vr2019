using System;
using UnityEngine;

public class KeyMonitor : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        detectPressedKeyOrButton();
    }

    public void detectPressedKeyOrButton()
    {

        /*foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }*/
    }
}
