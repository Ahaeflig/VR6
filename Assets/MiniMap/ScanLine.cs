using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanLine : MonoBehaviour {

	public Camera mainCamera;
	public GameObject robotBlip;
	public float scanSpeed;
	public float thresholdMaxDetection;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (mainCamera.WorldToViewportPoint(transform.position).y > 1 ) {
			Vector3 newPos = mainCamera.ViewportToWorldPoint (new Vector3 (mainCamera.WorldToViewportPoint (transform.position).x, 0, mainCamera.WorldToViewportPoint (transform.position).z));
			transform.position = newPos;
		}


	

		var scanLineZ = transform.position.z;
		var robotBlipZ = robotBlip.transform.position.z;
		var deltaZ = Mathf.Abs (scanLineZ - robotBlipZ);

		/*print ("line");
		print (scanLineZ);
		print("blip");
		print (robotBlipZ);
		print("delta");
		print (deltaZ);
		*/

		if (scanLineZ >= robotBlipZ && deltaZ <= thresholdMaxDetection) {
			var color = robotBlip.GetComponent<MeshRenderer> ().material.color;
			var progress = 1 - (deltaZ/thresholdMaxDetection);
			robotBlip.GetComponent<MeshRenderer> ().material.color = new Color (color.r, color.g, color.b, Mathf.Lerp (0f, 1f, progress));
		} else {
			var color = robotBlip.GetComponent<MeshRenderer> ().material.color;
			robotBlip.GetComponent<MeshRenderer> ().material.color = new Color (color.r, color.g, color.b, 0.0f);
		}
			
		transform.Translate (0, 0, Time.deltaTime * scanSpeed);
	}


}
