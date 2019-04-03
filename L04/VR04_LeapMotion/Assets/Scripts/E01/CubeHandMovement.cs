using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Leap;
using Leap.Unity;

public class CubeHandMovement : MonoBehaviour
{
    public LeapServiceProvider sp;

    // Update is called once per frame
    void Update()
    {
        // Get a new Frame
        Frame f = sp.CurrentFrame;
        if(f.Hands.Count > 0)
        {
            transform.position = f.Hands[0].PalmPosition.ToVector3();
        }
    }
}
