using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class Player : MonoBehaviour {

	public void SendRobotPosition() {
		RobotPositionMessage msg = new RobotPositionMessage();
		msg.position = this.transform.position;
		NetworkServer.SendToAll(NetworkMessageType.RobotPosition, msg);
	}

	public void HandleGetRobotPositionMessage(NetworkMessage n) {
		InvokeRepeating("SendRobotPosition", 0.0f, 1.0f);
	}
		
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetRobotPosition, HandleGetRobotPositionMessage);
	}

	void Update () {
		
	}
		

}
