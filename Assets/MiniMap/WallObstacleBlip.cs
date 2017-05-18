using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WallObstacleBlip : MonoBehaviour {

	public GameObject miniMap;
	public ClientNetwork clientNetwork;
	private bool hasBeenTrigered = false;

	public void Start () {
		GetComponent<Renderer> ().material.color = new Color (GetComponent<Renderer> ().material.color.r, GetComponent<Renderer> ().material.color.g, GetComponent<Renderer> ().material.color.b, 0.4f);
	}
		
	void Update() {

	}

	void LateUpdate () {
	}

	void OnMouseDown() {
		if (miniMap.GetComponent<MiniMap>().gameIsRunning) {
			RaycastHit hit; 
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				TriggerObstacle ();
			}
		}
	}

	public void TriggerObstacle() {
		if (!hasBeenTrigered) {
			hasBeenTrigered = true;
			TriggerWallObstacleMessage msg = new TriggerWallObstacleMessage ();
			msg.name = this.transform.name;
			clientNetwork.myClient.Send (NetworkMessageType.TriggerWallObstacle, msg);
			GetComponent<Renderer> ().material.color = new Color (GetComponent<Renderer> ().material.color.r, GetComponent<Renderer> ().material.color.g, GetComponent<Renderer> ().material.color.b, 1.0f);
		}
	}


}
