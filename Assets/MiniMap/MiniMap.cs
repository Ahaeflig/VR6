 using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public ClientNetwork clientNetwork;
	public GameObject obstacles;
	public GameObject wallObstacleBlip;

	private float terrainWidth;
	private float terrainHeight;

	public void SetWallObstacles(NetworkMessage netMsg) 
	{

		Debug.Log ("SetWallObstacles");
		WallObstacleMessage msg = netMsg.ReadMessage<WallObstacleMessage>();
		GameObject blip = Instantiate(wallObstacleBlip);

		blip.GetComponent<WallObstacleBlip>().transform.SetParent (obstacles.transform);

		var newPosition = getMapCoordinateForTarget (msg.position);
		blip.GetComponent<RectTransform> ().localPosition = new Vector3 (newPosition.x, newPosition.y, blip.GetComponent<RectTransform> ().localPosition.z);

		var newBlipSize = getMapCoordinateForTarget (msg.size);
		blip.GetComponent<RectTransform>().sizeDelta = new Vector2 (newBlipSize.x , newBlipSize.y);

		blip.GetComponent<RectTransform>().localScale = new Vector3 (1, 1, 1);
	
		blip.transform.name = msg.name;

		blip.GetComponent<WallObstacleBlip>().clientNetwork = clientNetwork;

		clientNetwork.myClient.Send(NetworkMessageType.GetRobotPosition, new EmptyMessage() );

	}

	public void GetTerrain(NetworkMessage netMsg)
	{
		TerrainMessage msg = netMsg.ReadMessage<TerrainMessage>();
		terrainWidth = msg.width;
		terrainHeight = msg.height;
		clientNetwork.myClient.Send(NetworkMessageType.GetWallObstacles, new EmptyMessage() );
	}

	// Use this for initialization
	void Start () 
	{
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log ("click!");
			var mousePosition=Input.mousePosition;
			Debug.Log(this.GetComponent<RectTransform> ().InverseTransformPoint (mousePosition));
		}
	}

	public Vector3 getMapCoordinateForTarget(Vector3 target)
	{
		var mapWidth = this.GetComponent<RectTransform>().rect.width;
		var mapHeight = this.GetComponent<RectTransform>().rect.height;
		var blipX =  MapInterval (target.x, -terrainWidth/2f, terrainWidth/2f, -mapWidth/2f, mapWidth/2f);
		var blipY =  MapInterval (target.z, -terrainHeight/2f, terrainHeight/2f, -mapHeight/2f, mapHeight/2f);
		return new Vector3 (blipX, blipY, this.GetComponent<RectTransform>().localPosition.z);
	}

	float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax) {
		if (val>=srcMax) return dstMax;
		if (val<=srcMin) return dstMin;
		return dstMin + (val-srcMin) / (srcMax-srcMin) * (dstMax-dstMin);
	} 

}
