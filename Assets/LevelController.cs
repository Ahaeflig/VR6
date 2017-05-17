using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LevelController : MonoBehaviour {

	public void SendBlocks(NetworkMessage nm)
	{
		Debug.Log ("SendBlocks");
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("block");
		foreach (var block in blocks) {
			BlockMessage msg = new BlockMessage();
			msg.position = block.transform.position;
			msg.id = block.GetInstanceID(); 
			msg.materialName = block.GetComponent<Renderer>().material.name;
			NetworkServer.SendToAll(NetworkMessageType.Block, msg);
		}
	}

	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetBlocks, SendBlocks);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
