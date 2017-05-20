using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class WallObstacle : MonoBehaviour {


	public float maxY;
	public float minY;
	public float waitBeforeGoingDown = 3f; // sec
	private bool isBeingAnimated = false;
	private bool wallIsGoingUp =true;

	// Use this for initialization
	void Start () {
		
	}


	public void Trigger() {
		if (!isBeingAnimated) {
			wallIsGoingUp = true;
			isBeingAnimated = true;
		}
	}

	void goDown() {
		wallIsGoingUp = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isBeingAnimated) {
			if (wallIsGoingUp && this.transform.position.y < maxY) {
				this.transform.Translate (0, 20 * Time.deltaTime, 0);
			} else if (wallIsGoingUp) {
				InvokeRepeating("goDown", waitBeforeGoingDown, 0.0f);
			}

			if (!wallIsGoingUp && this.transform.position.y > minY) {
				this.transform.Translate (0, -20 * Time.deltaTime, 0);
			} else if (!wallIsGoingUp) {
				WallObstacleHasFinishedMessage msg = new WallObstacleHasFinishedMessage ();
				msg.name = transform.name;
				NetworkServer.SendToAll (NetworkMessageType.WallObstacleHasFinished, msg);
			} 
		}
	}
}
