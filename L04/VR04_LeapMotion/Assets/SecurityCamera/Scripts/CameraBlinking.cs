using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBlinking : MonoBehaviour {


    public Renderer ledRenderer;

    public float blinkingSpeed = 1f;

	// Use this for initialization
	void Start () {
        Invoke("LedOn", blinkingSpeed);
    }

    public void LedOn()
    {
        ledRenderer.material.SetColor("_EmissionColor", Color.red);
        Invoke("LedOff", blinkingSpeed);
    }

    public void LedOff()
    {
        ledRenderer.material.SetColor("_EmissionColor", Color.black);
        Invoke("LedOn", blinkingSpeed);
    }
}
