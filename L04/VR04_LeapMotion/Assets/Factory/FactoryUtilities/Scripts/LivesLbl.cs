using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesLbl : MonoBehaviour {

	const string prefix = "LIVES: ";
	Text scoreLbl;

	// Use this for initialization
	void Start () {
		scoreLbl = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreLbl.text = prefix + GameManager.instance.lives.ToString();
	}
}
