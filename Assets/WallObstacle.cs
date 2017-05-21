using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class WallObstacle : MonoBehaviour {

	public bool interactive = false;
	public float maxY; 
	public float minY;
	public float waitBeforeGoingDown = 3f; // sec
	private bool isBeingAnimated = false;
	private WallState wallState = WallState.Down;

	enum WallState {
		Down,
		GoingUp,
		Up,
		GoingDown
	}

	// Use this for initialization
	void Start () {
		
	}




	public void Trigger() {
		if (!isBeingAnimated) {
			isBeingAnimated = true;
			wallState = WallState.GoingUp;
		}
	}


	IEnumerator WaitBeforeGoingDown() {
		wallState = WallState.Up;
		yield return new WaitForSeconds(waitBeforeGoingDown);
		wallState = WallState.GoingDown;
	}

	void OnTriggerEnter(Collider other) {
		if (other.CompareTag ("Bullet")) {
			if (this.transform.position.y > minY) {
				this.transform.Translate (0, -4 * Time.deltaTime, 0);
			} else {
				done ();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isBeingAnimated) {
			if (!interactive) {
				if (wallState == WallState.GoingUp && this.transform.position.y < maxY) {
					this.transform.Translate (0, 20 * Time.deltaTime, 0);
				} else if (wallState == WallState.GoingUp) {
					StartCoroutine (WaitBeforeGoingDown ());
				}
					
				if (wallState == WallState.GoingDown && this.transform.position.y > minY) {
					this.transform.Translate (0, -20 * Time.deltaTime, 0);
				} else if (wallState == WallState.GoingDown) {
					done ();
				} 
			} else {
				if (wallState == WallState.GoingUp && this.transform.position.y < maxY) {
					this.transform.Translate (0, 20 * Time.deltaTime, 0);
				} else if (wallState == WallState.GoingUp) {
					wallState = WallState.Up;
				}
			}
		}
	}

	void done() {
		isBeingAnimated = false;
		wallState = WallState.Down;
		WallObstacleHasFinishedMessage msg = new WallObstacleHasFinishedMessage ();
		msg.name = transform.name;
		NetworkServer.SendToAll (NetworkMessageType.WallObstacleHasFinished, msg);
	}
		
}