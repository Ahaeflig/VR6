 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

	public GameObject plane;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public Vector3 getMapCoordinateForTarger(Vector3 target)
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
