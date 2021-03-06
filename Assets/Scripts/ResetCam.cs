using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCam : MonoBehaviour {

	[SerializeField]
	GameObject Vehicle;

	[SerializeField]
	GameObject WantedPos;

	[SerializeField]
	GameObject CamHolder;

	IEnumerator resetCamPosEnumerator;

	// Use this for initialization
	void Start () {
		//resetCamPosEnumerator = ResetCamPos();
		//StartCoroutine(resetCamPosEnumerator);

		Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		Valve.VR.OpenVR.Compositor.SetTrackingSpace (
			Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);

		CamHolder.transform.position = WantedPos.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	

	}


	void LateUpdate() {


		//Quaternion temp = Vehicle.transform.rotation;
		//Vehicle.transform.rotation = Quaternion.identity;

		//print ("Called");


		/*Valve.VR.OpenVR.System.ResetSeatedZeroPose ();
		Valve.VR.OpenVR.Compositor.SetTrackingSpace (
			Valve.VR.ETrackingUniverseOrigin.TrackingUniverseSeated);
		*/


		CamHolder.transform.position = WantedPos.transform.position;

		CamHolder.transform.rotation = Vehicle.transform.rotation;

		//print (Vehicle.transform.rotation.y);


		//HMD_pos.transform.rotation = Quaternion.Euler (HMD_pos.transform.rotation.x, .transform.rotation.y, HMD_pos.transform.rotation.z);

		//Quaternion initialPose = HMD_pos.transform.rotation

		//Vehicle.transform.rotation = temp;
	}

}
