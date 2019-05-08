using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreLbl : MonoBehaviour {

	const string prefix = "SCORE: ";
	Text scoreLbl;

	// Use this for initialization
	void Start () {
		scoreLbl = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreLbl.text = prefix + GameManager.instance.score.ToString();
	}
}
