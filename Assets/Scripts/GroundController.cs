using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour {

	public bool isRotating = false;
	public float rotationSpeed = 5f;

	// Use this for initialization
	void Start () {
		
	}

	public void Rotate() {
		isRotating = true;
	}
		
	// Update is called once per frame
	void Update () {
		if (isRotating) {
			transform.Rotate (rotationSpeed * Time.deltaTime, 0, 0);
		}
	}

}


