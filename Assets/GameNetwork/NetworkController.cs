using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkController : MonoBehaviour {

	public GameObject prefabNetworkManager;
	private NetworkManager networkManager;
	public string networkAddress = "localhost";
	private bool isHosting = false;

	// Use this for initialization
	void Start () {
		networkManager = prefabNetworkManager.GetComponent<NetworkManager> ();
		networkManager.networkAddress = networkAddress;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI() {
		if (!isHosting) {
			networkAddress = GUI.TextField (new Rect (10, 10, 200, 20), networkAddress, 25);
			if (GUI.Button (new Rect (10, 70, 50, 30), "Host")) {
				networkManager.networkAddress = networkAddress;
				networkManager.StartHost ();
				isHosting = true;
			}
		}
	}

}
