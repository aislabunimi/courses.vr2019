using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KinectV2Components;

// GameManager handles all game logic
public class GameManager : MonoBehaviour {

	// Using Singleton pattern here...

	static GameManager _instance;

	[Header("Attachable")]
	public ItemSpawner spawner;
	public ConveyBeltMovement conveyBelt;
	public GameObject goodItemPrefab;
	public GameObject badItemPrefab;

	public GameObject gameOverPanel;
	public Text finalScoreTextField;

	[Header("Game Parameters")]
	[Range(0f,1f)]
	public float badItemSpawnProbability = 0.1f;
	[Range(0.1f,5f)]
	public float spawnRate = 0.5f;
	[Range(0.1f, 10f)]
	public float conveyBeltSpeed = 2f;

	[Header("Stats")]
	public int score = 0;
	public int lives = 3;

	[Header("Kinect parameters")]
	public float power;
	public KinectPositionBuffer jointBuffer;

	bool gameOver;

	static public GameManager instance {
		get{
			return _instance;
		}
	}

	void Awake(){
		_instance = this;
	}

	// Use this for initialization
	void Start () {
		spawner.DisableAutoSpawn();
		Invoke("RequestSpawn", 1f / spawnRate);
	}
	
	// Update is called once per frame
	void Update () {
		conveyBelt.scrollSpeed = conveyBeltSpeed;
	}

	void RequestSpawn(){
		// We are overriding autospawn behaviour from the ItemSpawner
		// because we want to randomly spawn good or bad items
		if( Random.Range(0f,1f) <= badItemSpawnProbability){
			spawner.SpawnItem(badItemPrefab);
		}
		else{
			spawner.SpawnItem(goodItemPrefab);
		}
		Invoke("RequestSpawn", 1f / spawnRate);
	}

	void RemoveLife(){
		lives--;
		// If we have no lives left, game is over
		if(lives <= 0){
			GameOver();
		}
	}

	void GameOver(){
		// We should deactivate everything in our scene
		gameOver = true;
		conveyBeltSpeed = 0;
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		foreach(GameObject item in items){
			item.SendMessage("SelfDestroy");
		}
		// Then display the gameover panel
		gameOverPanel.SetActive(true);
		finalScoreTextField.text = score.ToString();
	}

	// A good item reached distribution
	public void GoodItemReachedTheEnd(){
		score++;
	}

	// A bad item reached distribution
	public void BadItemReachedTheEnd(){
		RemoveLife();
	}

	// A good item was removed from production
	public void BadItemRemoved(){
		score++;
	}

	// A good item was removed from production
	public void GoodItemRemoved(){
		RemoveLife();
	}

}
