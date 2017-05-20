 using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

	public ClientNetwork clientNetwork;
	public Material blackMaterial;
	public Material whiteMaterial;
	public Material orangeMaterial;
	public Material greenMaterial;
	public GameObject square;
	public GameObject wallObstacle;
	public GameObject robotBlip;
	public Text message;
	public Transform parent;
	public bool gameIsRunning = false;

	public void HandleStartMessage(NetworkMessage netMsg) {
		Debug.Log ("Start");
		message.text = "";
		gameIsRunning = true;
	}

	public void HandleFinishMessage(NetworkMessage netMsg) {
		Debug.Log ("Finish");
		message.text = "You've lost!";
		gameIsRunning = false;
	}

	public void HandleGameOverMessage(NetworkMessage netMsg) {
		Debug.Log ("GameOver");
		message.text = "You've won!";
		gameIsRunning = false;
	}

	public void HandleWallObstacleHasFinished(NetworkMessage netMsg) {
		Debug.Log ("HandleWallObstacleHasFinished");
		WallObstacleHasFinishedMessage msg = netMsg.ReadMessage<WallObstacleHasFinishedMessage> ();
		WallObstacleBlip wallObstacleBlip = GameObject.Find (msg.name).GetComponent<WallObstacleBlip>();
		wallObstacleBlip.GetComponent<Renderer> ().material.color = new Color (GetComponent<Renderer> ().material.color.r, GetComponent<Renderer> ().material.color.g, GetComponent<Renderer> ().material.color.b, 0.5f);
	}

	public void SetBlocks(NetworkMessage netMsg) {
		Debug.Log ("SetBlocks");
		BlockMessage msg = netMsg.ReadMessage<BlockMessage>();
		GameObject blip = Instantiate(square);
		blip.transform.SetParent (parent);
		blip.transform.position = msg.position;
		blip.transform.localScale = msg.size;
		blip.transform.name = msg.name;
		if (msg.materialName == "Black (Instance)") {
			blip.GetComponent<Renderer> ().material = blackMaterial;
		} else if (msg.materialName == "White (Instance)") {
			blip.GetComponent<Renderer> ().material = whiteMaterial;
		} else if (msg.materialName == "Orange (Instance)") {
			blip.GetComponent<Renderer> ().material = orangeMaterial;
		} else if (msg.materialName == "Green (Instance)") {
			blip.GetComponent<Renderer> ().material = greenMaterial;
		}
	}
		
	public void SetPlatforms(NetworkMessage netMsg) {
		Debug.Log ("SetPlatforms");
		PlateformMessage msg = netMsg.ReadMessage<PlateformMessage>();
		GameObject blip = Instantiate(square);
		blip.transform.SetParent (parent);
		blip.transform.position = msg.position;
		blip.transform.localScale = msg.size / 10f;
		blip.transform.name = msg.name;
		if (msg.materialName == "Black (Instance)") {
			blip.GetComponent<Renderer> ().material = blackMaterial;
		} else if (msg.materialName == "White (Instance)") {
			blip.GetComponent<Renderer> ().material = whiteMaterial;
		} else if (msg.materialName == "Orange (Instance)") {
			blip.GetComponent<Renderer> ().material = orangeMaterial;
		} else if (msg.materialName == "Green (Instance)") {
			blip.GetComponent<Renderer> ().material = greenMaterial;
		}
	}



	public void SetWallObstacles(NetworkMessage netMsg) {
		Debug.Log ("SetWallObstacles");
		WallObstacleMessage msg = netMsg.ReadMessage<WallObstacleMessage>();
		GameObject blip = Instantiate(wallObstacle) ;
		blip.transform.SetParent (parent);
		blip.transform.position = new Vector3(msg.position.x, -23.0f, msg.position.z);
		blip.transform.localScale = msg.size / 10.0f;
		blip.transform.name = msg.name;
		blip.transform.rotation = msg.rotation;
		blip.GetComponent<WallObstacleBlip>().clientNetwork = clientNetwork;
		blip.GetComponent<WallObstacleBlip>().miniMap = gameObject;
		message.text = "waiting for player...";
	}




	public void SetRobotPosition(NetworkMessage netMsg) {
		Debug.Log ("SetRobotPosition");
		RobotPositionMessage msg = netMsg.ReadMessage<RobotPositionMessage>();
		robotBlip.transform.position = new Vector3(msg.position.x, -23.0f, msg.position.z);
	}
		


	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		/*if (Input.GetMouseButtonDown(0))
		{
			Debug.Log ("click!");
			var mousePosition=Input.mousePosition;
			Debug.Log(this.GetComponent<RectTransform> ().InverseTransformPoint (mousePosition));
		}*/
	}



}
