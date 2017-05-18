using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WallObstacleBlip : MonoBehaviour {

	public ClientNetwork clientNetwork;

	public void Start () 
	{
	}
		
	void Update() {

	}

	void LateUpdate () {
	}

	void OnMouseDown() {
		RaycastHit hit; 
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
		if (Physics.Raycast (ray, out hit, 100.0f)) {
			TriggerObstacle ();
		}
	}

	public void TriggerObstacle() {
		TriggerWallObstacleMessage msg = new TriggerWallObstacleMessage();
		msg.name = this.transform.name;
		clientNetwork.myClient.Send(NetworkMessageType.TriggerWallObstacle, msg);
	}
}
