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
		Debug.Log (col.gameObject.name);
		NetworkServer.SendToAll (NetworkMessageType.Finish, new EmptyMessage ());
	
	}

}
