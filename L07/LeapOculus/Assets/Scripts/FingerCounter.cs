using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;

[RequireComponent(typeof(Text))]
public class FingerCounter : MonoBehaviour
{

    public LeapServiceProvider leapServiceProvider;
    Text txt;

    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Frame f = leapServiceProvider.CurrentFrame;
        counter = 0;
        for (int j = 0; j < f.Hands.Count; j++)
        {
            for (int i = 0; i < f.Hands[j].Fingers.Count; i++)
            {
                Finger fing = f.Hands[j].Fingers[i];
                if (fing.IsExtended) counter++;
            }
        }
        txt.text = "" + counter;
    }
}
