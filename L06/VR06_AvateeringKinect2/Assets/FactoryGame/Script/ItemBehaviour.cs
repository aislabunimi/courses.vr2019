using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehaviour : MonoBehaviour {

	public bool isGoodItem = false;
	public bool isBadItem = false;

	ConveyBeltMovement cbm;
	CameraLookAt cla;
	Rigidbody rb;
	Vector3 currentPos = Vector3.zero;

	bool onTapis = false;

	// Use this for initialization
	void Start () {
		cbm = FindObjectOfType<ConveyBeltMovement>();
		cla = FindObjectOfType<CameraLookAt>();
		currentPos = transform.position;
		rb = GetComponent<Rigidbody>();
		if(cbm == null){
			Debug.LogError("CONVEY BELT NOT FOUND IN SCENE");
		}
		if (cla == null){
			Debug.LogError("NO CAMERA LOOK AT FOUND");
		}
		else{
			if(cla.target == null){
				// If camera on the background is not looking at something
				// we tell it to look at current item...
				cla.target = transform;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (cbm != null){
			UpdatePosition();
		}
	}

	void UpdatePosition(){
		// Move the object with the tapis roulant only if we are on it!
		if(onTapis){
			currentPos.x -= cbm.scrollSpeed * Time.deltaTime;
			transform.position = currentPos;
		}
	}

	void OnTriggerEnter(Collider col){
		// Detect collisions with destroyer
		if(col.gameObject.CompareTag("Destroyer")){
			if(isGoodItem){
				GameManager.instance.GoodItemReachedTheEnd();
			}
			if(isBadItem){
				GameManager.instance.BadItemReachedTheEnd();
			}
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision col){
		// Detect all collision
		if(col.gameObject.name == "ConveyBelt"){
			onTapis = true;
		}

		// Leap motion only
		//if(col.gameObject.CompareTag("Hand")){
		//	onTapis = false;
		//}

		// Kinect only
		if (col.gameObject.CompareTag("AvatarJoint"))
		{
			onTapis = false;
			rb.AddForce(GameManager.instance.jointBuffer.GetDirection(col.gameObject.name) 
				* GameManager.instance.power, ForceMode.Impulse);
			rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
		}

		if(col.gameObject.name == "FactoryPrefab"){
			// If we fall outside the tapis roulant we should destroy ourselves
			if(isGoodItem){
				GameManager.instance.GoodItemRemoved();
			}
			if(isBadItem){
				GameManager.instance.BadItemRemoved();
			}
			Invoke("SelfDestroy", 3f);
		}
	}

	public void SelfDestroy(){
		Destroy(gameObject);
	}
}
