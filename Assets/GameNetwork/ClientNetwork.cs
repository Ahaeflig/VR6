using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;


public class ClientNetwork : MonoBehaviour {

	public MiniMap miniMap;
	public NetworkClient myClient;

	public void SetupClient() {
		myClient = new NetworkClient();
		myClient.RegisterHandler(MsgType.Connect, OnConnected);
		myClient.RegisterHandler (NetworkMessageType.Block, miniMap.SetBlocks);
		myClient.RegisterHandler (NetworkMessageType.Plateform, miniMap.SetPlatforms);
		myClient.RegisterHandler (NetworkMessageType.WallObstacles, miniMap.SetWallObstacles);
		myClient.RegisterHandler (NetworkMessageType.RobotPosition, miniMap.SetRobotPosition);
		myClient.RegisterHandler (NetworkMessageType.Start, miniMap.HandleStartMessage);
		myClient.RegisterHandler (NetworkMessageType.Finish, miniMap.HandleFinishMessage);
		myClient.RegisterHandler (NetworkMessageType.GameOver, miniMap.HandleGameOverMessage);
		myClient.RegisterHandler (NetworkMessageType.WallObstacleHasFinished, miniMap.HandleWallObstacleHasFinished);
		myClient.RegisterHandler (NetworkMessageType.StartPlateform, miniMap.SetStartPlateforms);

		myClient.Connect(NetworkInfo.serverIp, NetworkInfo.serverPort);
	}
		
		
	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log ("ClientNetwork OnConnected");
		myClient.Send (NetworkMessageType.GetBlocks, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetStartPlateforms, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetPlateforms, new EmptyMessage());
	   	myClient.Send (NetworkMessageType.GetWallObstacles, new EmptyMessage());
		myClient.Send (NetworkMessageType.GetRobotPosition, new EmptyMessage());
	}
		
	// Use this for initialization
	void Start () {
		SetupClient ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
