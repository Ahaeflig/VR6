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
		InvokeRepeating("SendRobotPosition", 0.0f, 3.0f);
	}
		
	void Start () {
		NetworkServer.RegisterHandler(NetworkMessageType.GetRobotPosition, HandleGetRobotPositionMessage);
	}

	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
		var z = Input.GetAxis("Vertical") * Time.deltaTime * 100.0f;
		transform.Rotate(0, x, 0);
		transform.Translate(0, 0, z);
	}

}
