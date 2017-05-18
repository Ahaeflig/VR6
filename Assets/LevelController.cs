using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class LevelController : MonoBehaviour {

	public void SendBlocks(NetworkMessage netMsg)
	{
		Debug.Log ("SendBlocks");
		GameObject[] blocks = GameObject.FindGameObjectsWithTag ("block");
		foreach (var block in blocks) {
			string blockNewName = "" + block.GetInstanceID ();
			block.transform.name = blockNewName;
			BlockMessage msg = new BlockMessage();
			msg.position = block.transform.position;
			msg.size = block.transform.localScale;
			msg.name = blockNewName; 
			msg.materialName = block.GetComponent<Renderer>().material.name;
			NetworkServer.SendToAll(NetworkMessageType.Block, msg);
		}
	}

	public void SendWallObstacles(NetworkMessage netMsg)
	{
		Debug.Log ("SendWallObstacles");
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		foreach (var obstacle in obstacles) {
			string obstacleNewName = "" + obstacle.GetInstanceID ();
			obstacle.transform.name = obstacleNewName;
			WallObstacleMessage msg = new WallObstacleMessage();
			msg.position = obstacle.transform.position;
			msg.size = obstacle.transform.localScale;
			msg.name = obstacleNewName; 
			NetworkServer.SendToAll(NetworkMessageType.WallObstacles, msg);
		}
	}

	public void TriggerWallObstacle(NetworkMessage netMsg) {
		TriggerWallObstacleMessage msg = netMsg.ReadMessage<TriggerWallObstacleMessage>();
		WallObstacle wallObstacle = this.transform.Find (msg.name).GetComponent<WallObstacle>();
		wallObstacle.Trigger ();
	}


	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetBlocks, SendBlocks);
		NetworkServer.RegisterHandler(NetworkMessageType.GetWallObstacles, SendWallObstacles);
		NetworkServer.RegisterHandler(NetworkMessageType.TriggerWallObstacle, TriggerWallObstacle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
