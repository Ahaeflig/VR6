using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : MonoBehaviour {

	public GameObject target;
	public MiniMap map;
	protected RectTransform myRectTransform;


	// Use this for initialization
	void Start () {
		myRectTransform = GetComponent<RectTransform> ();
	}

	protected void LateUpdate () {
		var newBlipPosition = map.getMapCoordinateForTarget (target.transform.position);
		myRectTransform.localPosition = new Vector3 (newBlipPosition.x, newBlipPosition.y, myRectTransform.localPosition.z);
	}

   
}
