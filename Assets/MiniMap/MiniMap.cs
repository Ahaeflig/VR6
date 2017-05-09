 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public GameObject plane;
	public GameObject obstacleBlip;
	public GameObject wallBlip;



	// Use this for initialization
	void Start () {
		/*Transform[] worldChildren = GameObject.Find("World").GetComponentsInChildren<Transform>();
		foreach (var child in worldChildren) 
		{
			Debug.Log (child.GetType());
		}*/
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag ("obstacle");
		foreach (var obstacle in obstacles)
		{
			ObstacleBlip blip = Instantiate(obstacleBlip).GetComponent<ObstacleBlip>();
			blip.transform.SetParent (this.transform);
			blip.target = obstacle;
			blip.map = this;
        }

		GameObject[] walls = GameObject.FindGameObjectsWithTag ("wall");
		foreach (var wall in walls)
		{
			WallBlip blip = Instantiate(wallBlip).GetComponent<WallBlip>();
			blip.transform.SetParent (this.transform);
			blip.target = wall;
			blip.map = this;
		}
			
	}

	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 getMapCoordinateForTarget(Vector3 target)
	{

		var worldWidth = plane.GetComponent<Renderer> ().bounds.size.x;
		var worldHeight = plane.GetComponent<Renderer> ().bounds.size.z;

		var mapWidth = this.GetComponent<RectTransform>().rect.width;
		var mapHeight = this.GetComponent<RectTransform>().rect.height;

		var blipX =  MapInterval (target.x, -worldWidth/2f, worldWidth/2f, -mapWidth/2f, mapWidth/2f);
		var blipY =  MapInterval (target.z, -worldHeight/2f, worldHeight/2f, -mapHeight/2f, mapHeight/2f);

		return new Vector3 (blipX, blipY, this.GetComponent<RectTransform>().localPosition.z);
	}

	float MapInterval(float val, float srcMin, float srcMax, float dstMin, float dstMax) {
		if (val>=srcMax) return dstMax;
		if (val<=srcMin) return dstMin;
		return dstMin + (val-srcMin) / (srcMax-srcMin) * (dstMax-dstMin);
	} 






}
