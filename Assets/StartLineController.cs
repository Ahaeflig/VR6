using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StartLineController : MonoBehaviour {

	public bool hasGameStarted = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col) {
		Debug.Log (col.gameObject.name);
		hasGameStarted = true;
		NetworkServer.SendToAll(NetworkMessageType.Start, new EmptyMessage());
		gameObject.SetActive (false);
	}
}
