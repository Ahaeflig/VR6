using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class RobotBlip : MonoBehaviour {

	public MiniMap map;

	public void SetRobotPosition(NetworkMessage netMsg)
	{
		Debug.Log ("SetRobotPosition");
		RobotPositionMessage msg = netMsg.ReadMessage<RobotPositionMessage>();
		var newRobotPosition = map.getMapCoordinateForTarget (msg.position);
		GetComponent<RectTransform> ().localPosition = new Vector3 (newRobotPosition.x, newRobotPosition.y, GetComponent<RectTransform> ().localPosition.z);
	}
		
	// Use this for initialization
	void Start () {
		
	}

	void LateUpdate () {
		
	}

   
}
