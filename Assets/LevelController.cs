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

	public void SendStartPlateforms(NetworkMessage netMsg)
	{
		GameObject[] platforms = GameObject.FindGameObjectsWithTag ("startPlateform");
		foreach (var platform in platforms) {
			string blockNewName = "" + platform.GetInstanceID ();
			platform.transform.name = blockNewName;
			BlockMessage msg = new BlockMessage();
			msg.position = transform.position;
			msg.size = platform.transform.localScale;
			msg.name = blockNewName; 
			msg.materialName = platform.GetComponent<Renderer>().material.name;
			NetworkServer.SendToAll(NetworkMessageType.StartPlateform, msg);
		}

	}

	public void SendPlateforms(NetworkMessage netMsg)
	{
		GameObject[] platforms = GameObject.FindGameObjectsWithTag ("plateform");
		foreach (var platform in platforms) {
			string blockNewName = "" + platform.GetInstanceID ();
			platform.transform.name = blockNewName;
			BlockMessage msg = new BlockMessage();

			if (platform.GetComponent<FinishController> () != null) {
				msg.position = platform.GetComponent<FinishController> ().initPosition;
			} else {
				msg.position = transform.position;
			}

			msg.size = platform.transform.localScale;
			msg.name = blockNewName; 
			msg.materialName = platform.GetComponent<Renderer>().material.name;
			NetworkServer.SendToAll(NetworkMessageType.Plateform, msg);
		}

	}


	public void SendWallObstacles(NetworkMessage netMsg)
	{
		Debug.Log ("SendWallObstacles");
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		var counter = 0;
		foreach (var obstacle in obstacles) {
			string obstacleNewName = "wallObstacle" + (++counter);
			obstacle.transform.name = obstacleNewName;
			WallObstacleMessage msg = new WallObstacleMessage();
			msg.position = obstacle.transform.position;
			msg.size = obstacle.transform.localScale;
			msg.name = obstacleNewName; 
			msg.rotation = obstacle.transform.rotation;
			NetworkServer.SendToAll(NetworkMessageType.WallObstacles, msg);
		}
	}


	public void TriggerWallObstacle(NetworkMessage netMsg) {
		Debug.Log ("TriggerWallObstacle");
		TriggerWallObstacleMessage msg = netMsg.ReadMessage<TriggerWallObstacleMessage>();
		WallObstacle wallObstacle = GameObject.Find (msg.name).GetComponent<WallObstacle>();
		wallObstacle.Trigger ();
	}


	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetBlocks, SendBlocks);
		NetworkServer.RegisterHandler(NetworkMessageType.GetWallObstacles, SendWallObstacles);
		NetworkServer.RegisterHandler(NetworkMessageType.GetPlateforms, SendPlateforms);
		NetworkServer.RegisterHandler(NetworkMessageType.TriggerWallObstacle, TriggerWallObstacle);

		RenderSettings.fog = true;
		RenderSettings.fogMode = FogMode.ExponentialSquared;
		RenderSettings.fogDensity = 0.001f;
		RenderSettings.fogColor = Color.grey;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
