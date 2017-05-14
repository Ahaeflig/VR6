using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlaneController : MonoBehaviour {

	public void SendTerrain(NetworkMessage nm) {
		TerrainMessage msg = new TerrainMessage();
		msg.width = this.GetComponent<Renderer>().bounds.size.x;
		msg.height = this.GetComponent<Renderer>().bounds.size.z;
		NetworkServer.SendToAll(NetworkMessageType.Terrain, msg);
		Debug.Log ("Sent plane dim to minimap " + msg.width + " " + msg.height );
	}
		
	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetTerrain, SendTerrain);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
