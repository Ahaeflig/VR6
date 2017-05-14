using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Obstacles : MonoBehaviour {

	public void SendWallObstacles(NetworkMessage nm)
	{
		Debug.Log ("Received from minimap for obstacles ");
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		foreach (var obstacle in obstacles)
		{
			WallObstacleMessage msg = new WallObstacleMessage();
			msg.position = obstacle.transform.position;
			msg.size = obstacle.GetComponent<Renderer> ().bounds.size;
			NetworkServer.SendToAll(NetworkMessageType.WallObstacles, msg);
			Debug.Log ("Sent obstacle to minimap");
		}
	}

	// Use this for initialization
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetWallObstacles, SendWallObstacles);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
