using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlane : MonoBehaviour {

	Vector3 spawnPos;

	Quaternion spawnRot;

	// Use this for initialization
	void Start () {
		spawnPos = transform.GetChild(0).position;
		spawnRot = transform.GetChild (0).rotation;
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnTriggerEnter(Collider other) {


		if (other.CompareTag("Player")) {
			//TODO play fade screen 

			Rigidbody rigidbody = other.attachedRigidbody;

			rigidbody.isKinematic = true;

			//rigidbody.Sleep ();

			rigidbody.position = spawnPos;
			rigidbody.rotation = spawnRot;

			rigidbody.isKinematic = false;

			rigidbody.velocity = new Vector3 (0, 0, 0);

			//rigidbody.WakeUp ();
		}

}


}
