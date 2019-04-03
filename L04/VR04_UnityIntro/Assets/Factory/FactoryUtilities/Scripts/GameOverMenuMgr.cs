using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenuMgr : MonoBehaviour {

	public void OnRestartClicked(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void OnExitClicked(){
		Application.Quit();
	}
}
