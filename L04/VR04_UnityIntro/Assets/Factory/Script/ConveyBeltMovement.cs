using UnityEngine;
using System.Collections;

public class ConveyBeltMovement : MonoBehaviour {

    public float scrollSpeed = 0.5F;
    public Material mat;

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
