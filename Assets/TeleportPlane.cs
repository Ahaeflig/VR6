using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlane : MonoBehaviour {

	Vector3 spawnPos;

	// Use this for initialization
	void Start () {
		spawnPos = transform.GetChild(0).position;
		 
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnTriggerEnter(Collider other) {


		if (other.CompareTag("Player")) {
			//TODO play fade screen 
			Debug.Log("aie aie aie");
			Rigidbody rigidbody = other.attachedRigidbody;

			rigidbody.isKinematic = true;

			//rigidbody.Sleep ();

			rigidbody.position = spawnPos;

			rigidbody.isKinematic = false;

			rigidbody.velocity = new Vector3 (0, 0, 0);

			//rigidbody.WakeUp ();
		}

}


}
