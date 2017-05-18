 using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public ClientNetwork clientNetwork;
	public Material blackMaterial;
	public Material whiteMaterial;
	public GameObject square;
	public GameObject wallObstacle;
	public GameObject robotBlip;
	public Transform parent;


	public void SetBlocks(NetworkMessage netMsg) {
		Debug.Log ("SetBlocks");
		BlockMessage msg = netMsg.ReadMessage<BlockMessage>();
		GameObject blip = Instantiate(square);
		blip.transform.SetParent (parent);
		blip.transform.position = msg.position;
		blip.transform.localScale = msg.size / 10.0f;
		blip.transform.name = msg.name;
		if (msg.materialName == "Black (Instance)") {
			blip.GetComponent<Renderer> ().material = blackMaterial;
		} else {
			blip.GetComponent<Renderer> ().material = whiteMaterial;
		}
	}

	public void SetWallObstacles(NetworkMessage netMsg) {
		Debug.Log ("SetWallObstacles");
		WallObstacleMessage msg = netMsg.ReadMessage<WallObstacleMessage>();
		GameObject blip = Instantiate(wallObstacle);
		blip.transform.SetParent (parent);
		blip.transform.position = msg.position;
		blip.transform.localScale = msg.size / 10.0f;
		blip.transform.name = msg.name;
	}

	public void SetRobotPosition(NetworkMessage netMsg) {
		Debug.Log ("SetRobotPosition");
		RobotPositionMessage msg = netMsg.ReadMessage<RobotPositionMessage>();
		robotBlip.transform.position = new Vector3(msg.position.x, 10.0f, msg.position.z);
	}



	// Use this for initialization
	void Start () 
	{
		
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
