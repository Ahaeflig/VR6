 using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public ClientNetwork clientNetwork;
	public Material blackMaterial;
	public Material whiteMaterial;
	public GameObject square;
	public GameObject robotBlip;


	public void SetBlocks(NetworkMessage netMsg) {
		Debug.Log ("SetBlocks");
		BlockMessage msg = netMsg.ReadMessage<BlockMessage>();
		GameObject blip = Instantiate(square);
		blip.transform.position = msg.position;
		blip.transform.localScale = new Vector3 (1, 1, 1);
		blip.GetComponent<Square>().originalId = msg.id;
		if (msg.materialName == "Black (Instance)") {
			blip.GetComponent<Renderer> ().material = blackMaterial;
		} else {
			blip.GetComponent<Renderer> ().material = whiteMaterial;
		}
		clientNetwork.myClient.Send(NetworkMessageType.GetRobotPosition, new EmptyMessage() );
	}

	public void SetRobotPosition(NetworkMessage netMsg) {
		Debug.Log ("SetRobotPosition");
		RobotPositionMessage msg = netMsg.ReadMessage<RobotPositionMessage>();
		robotBlip.transform.position = new Vector3(msg.position.x, 3.0f, msg.position.z);
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
