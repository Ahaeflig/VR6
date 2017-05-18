using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class WallObstacle : MonoBehaviour {


	public float maxY;
	private bool isBeingAnimated = false;
	private bool hasBeenActivated = false;

	// Use this for initialization
	void Start () {
		
	}


	public void Trigger() {
		if (!hasBeenActivated) {
			hasBeenActivated = true;
			isBeingAnimated = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isBeingAnimated) {
			if (this.transform.position.y < maxY) {
				this.transform.Translate (0, 200f * Time.deltaTime, 0);
			} else {
				isBeingAnimated = false;
			}
		}
	}
}
