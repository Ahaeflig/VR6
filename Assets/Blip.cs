using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blip : MonoBehaviour {

	public GameObject target;
	public MiniMap map;
	RectTransform myRectTransform;


	// Use this for initialization
	void Start () {
		myRectTransform = GetComponent<RectTransform> ();
	}

	void LateUpdate () {
        Debug.Log(target);
		var newBlipPosition = map.getMapCoordinateForTarger (target.transform.position);
		myRectTransform.localPosition = new Vector3 (newBlipPosition.x, newBlipPosition.y, myRectTransform.localPosition.z);
	}

   
}
