using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class ClientNetwork : MonoBehaviour {

	public MiniMap miniMap;
	public NetworkClient myClient;
	private bool isConnected = false;
	private string networkAddress = "";

	public void SetupClient() {
		myClient = new NetworkClient();
		myClient.RegisterHandler (MsgType.Connect, OnConnected);
		myClient.RegisterHandler (NetworkMessageType.Block, miniMap.SetBlocks);
		myClient.RegisterHandler (NetworkMessageType.Plateform, miniMap.SetPlatforms);
		myClient.RegisterHandler (NetworkMessageType.WallObstacles, miniMap.SetWallObstacles);
		myClient.RegisterHandler (NetworkMessageType.RobotPosition, miniMap.SetRobotPosition);
		myClient.RegisterHandler (NetworkMessageType.Start, miniMap.HandleStartMessage);
		myClient.RegisterHandler (NetworkMessageType.Finish, miniMap.HandleFinishMessage);
		myClient.RegisterHandler (NetworkMessageType.GameOver, miniMap.HandleGameOverMessage);
		myClient.RegisterHandler (NetworkMessageType.WallObstacleHasFinished, miniMap.HandleWallObstacleHasFinished);
		myClient.RegisterHandler (NetworkMessageType.StartPlateform, miniMap.SetStartPlateforms);
		myClient.Connect(networkAddress, NetworkInfo.serverPort);
	}
		
		
	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log ("ClientNetwork OnConnected");
		myClient.Send (NetworkMessageType.GetBlocks, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetStartPlateforms, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetPlateforms, new EmptyMessage());
	   	myClient.Send (NetworkMessageType.GetWallObstacles, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetRobotPosition, new EmptyMessage());
	}


	void OnGUI() {
		if (!isConnected) {
			networkAddress = GUI.TextField (new Rect (10, 10, 200, 20), networkAddress, 25);
			if (GUI.Button (new Rect (10, 70, 200, 30), "Connect to Host")) {
				isConnected = true;
				SetupClient ();
			}
		}
	}
		
	// Use this for initialization
	void Start () {
		//SetupClient ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
