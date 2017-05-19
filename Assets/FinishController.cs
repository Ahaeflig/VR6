using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FinishController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnCollisionEnter(Collision col) {
		Debug.Log (col.gameObject.name);
		NetworkServer.SendToAll (NetworkMessageType.Finish, new EmptyMessage ());
	
	}

}
