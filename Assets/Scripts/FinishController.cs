using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FinishController : MonoBehaviour {


	public bool isSliding = false; 
	public bool isMovingUpDown = false; 
	public float speed = 10f;
	public float totalDistance = 30f;
	public float currentDistance = 0f;
	private int direction = 1;
	public Vector3 initPosition; 

	[SerializeField]
	GameObject LevelText; 

	// Use this for initialization
	void Start () {
		initPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (isSliding) {
			if (Mathf.Abs(currentDistance) < totalDistance) {
				transform.Translate (direction * transform.forward * speed * Time.deltaTime);
				currentDistance = currentDistance + (speed * Time.deltaTime);
			} else {
				direction = direction * 1 * -1;
				currentDistance = 0;
			}
		} else if (isMovingUpDown) {
			if (Mathf.Abs(currentDistance) < totalDistance) {
				transform.Translate (direction * transform.up * speed * Time.deltaTime);
				currentDistance = currentDistance + (speed * Time.deltaTime);
			} else {
				direction = direction * 1 * -1;
				currentDistance = 0;
			}
		}
	}
		
	void OnCollisionEnter(Collision col) {
		if (col.collider.CompareTag ("Player")) {
			Debug.Log (col.gameObject.name);

			LevelText.GetComponent<TextMesh> ().text = "You won";
			
			NetworkServer.SendToAll (NetworkMessageType.Finish, new EmptyMessage ());
		}	
	}

}
