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
		myClient.RegisterHandler (NetworkMessageType.RobotPosition, miniMap.SetRobotPosition);
		myClient.Connect(NetworkInfo.serverIp, NetworkInfo.serverPort);
	}
		
		
	public void OnConnected(NetworkMessage netMsg) {
		Debug.Log ("ClientNetwork OnConnected");
		myClient.Send (NetworkMessageType.GetBlocks, new EmptyMessage());
	}
		
	// Use this for initialization
	void Start () {
		SetupClient ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
