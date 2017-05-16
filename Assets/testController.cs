using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testController : MonoBehaviour {

	[SerializeField]
	GameObject controller1;
	[SerializeField]
	GameObject controller2;

	SteamVR_TrackedObject trackedControllerLeft;
	SteamVR_TrackedObject trackedControllerRight;

	SteamVR_Controller.Device controllerLeft;
	SteamVR_Controller.Device controllerRight;

	[SerializeField]
	float PITCH_ANGLE_RANGE = 35f;
	[SerializeField]
	float YAW_ANGLE_RANGE = 50f;
	[SerializeField]
	float ROLL_ANGLE_RANGE = 60f;

	[SerializeField]
	float PITCH_NEUTRAL_TOLERANCE = 15f;
	[SerializeField]
	float YAW_NEUTRAL_TOLERANCE = 15f;
	[SerializeField]
	float ROLL_NEUTRAL_TOLERANCE = 15f;

	Vector3 initRight = Vector3.right;
	Vector3 initUp = Vector3.up;
	Vector3 initForward = Vector3.forward;

	float forwardSpeed = 0f;
	float rightSpeed = 0f;
	float angularSpeed = 0f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
