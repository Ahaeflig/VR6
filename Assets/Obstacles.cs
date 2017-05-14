using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Obstacles : MonoBehaviour {

	public void SendWallObstacles(NetworkMessage nm)
	{
		Debug.Log ("Received from minimap for obstacles ");
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		foreach (var obstacle in obstacles) {
			WallObstacleMessage msg = new WallObstacleMessage();
			msg.position = obstacle.transform.position;
			msg.size = obstacle.GetComponent<Renderer> ().bounds.size;
			msg.name = obstacle.transform.name; 
			NetworkServer.SendToAll(NetworkMessageType.WallObstacles, msg);
			Debug.Log ("Sent obstacle to minimap");
		}
	}

	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetWallObstacles, SendWallObstacles);
		NetworkServer.RegisterHandler(NetworkMessageType.TriggerWallObstacle, TriggerWallObstacle);
	}



	public void TriggerWallObstacle(NetworkMessage netMsg) {
		TriggerWallObstacleMessage msg = netMsg.ReadMessage<TriggerWallObstacleMessage>();
		WallObstacle wallObstacle = this.transform.Find (msg.name).GetComponent<WallObstacle>();
		wallObstacle.Trigger ();
	}


	// Update is called once per frame
	void Update () {
		
	}
}
