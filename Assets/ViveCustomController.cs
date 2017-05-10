using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Turn this class into  a Singleton
public class ViveCustomController : MonoBehaviour {

	[SerializeField]
	GameObject controller1;
	[SerializeField]
	GameObject controller2;

	SteamVR_TrackedObject trackedControllerLeft;
	SteamVR_TrackedObject trackedControllerRight;

	SteamVR_Controller.Device controllerLeft;
	SteamVR_Controller.Device controllerRight;

	[SerializeField]
	GameObject cubeTest;

	[SerializeField]
	float PITCH_ANGLE_RANGE = 40f;
	[SerializeField]
	float YAW_ANGLE_RANGE = 110f;
	[SerializeField]
	float ROLL_ANGLE_RANGE = 150f;

	[SerializeField]
	float PITCH_NEUTRAL_TOLERANCE = 4f;
	[SerializeField]
	float YAW_NEUTRAL_TOLERANCE = 8f;
	[SerializeField]
	float ROLL_NEUTRAL_TOLERANCE = 8f;

	[SerializeField]
	float FORWARD_SPEED_MULTIPLIER = 1f;
	[SerializeField]
	float RIGHT_SPEED_MULTIPLIER = 1f;
	[SerializeField]
	float ANGULAR_SPEED_ROTATION_MULTIPLIER = 1f;

	Vector3 initRight = Vector3.right;
	Vector3 initUp = Vector3.up;
	Vector3 initForward = Vector3.forward;

	// Use this for initialization
	void Start () {
		InitControllers ();
	}

	void InitControllers() {

		// Left controller

		int controllerLeftIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);

		if (controllerLeftIndex == (int)controller1.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller1.GetComponent<SteamVR_TrackedObject> ();
		} else if (controllerLeftIndex == (int)controller2.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller2.GetComponent<SteamVR_TrackedObject> ();
		} else {
			trackedControllerLeft = null;
		}

		controllerLeft = SteamVR_Controller.Input ((int)trackedControllerLeft.index);

		// Right controller

		int controllerRightIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);

		if (controllerRightIndex == (int)controller1.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerRight = controller1.GetComponent<SteamVR_TrackedObject> ();
		} else if (controllerRightIndex == (int)controller2.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerRight = controller2.GetComponent<SteamVR_TrackedObject> ();
		} else {
			trackedControllerRight = null;
		}

		controllerRight = SteamVR_Controller.Input ((int)trackedControllerRight.index);
	}

	// Update is called once per frame
	void Update () {

		// Reset the initial position of the player if the grip button is clicked
		if (controllerRight.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			initRight = trackedControllerRight.transform.right;
			initUp = trackedControllerRight.transform.up;
			initForward = trackedControllerRight.transform.forward;

			InitControllers ();
		}
		if (controllerLeft.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			initRight = trackedControllerLeft.transform.right;
			initUp = trackedControllerLeft.transform.up;
			initForward = trackedControllerLeft.transform.forward;

			InitControllers ();
		}

		Vector3 positionIncrement = new Vector3(0, 0, 0);
		float rotationIncrement = 0;

		if (controllerLeft.GetHairTriggerDown ()) {
			// TODO Move robot's left arm


		} else {
			// Move robot
			Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerLeft.transform));

			float forwardSpeed = FORWARD_SPEED_MULTIPLIER * Time.deltaTime * GetForwardSpeed(movement.x);
			float rightSpeed = RIGHT_SPEED_MULTIPLIER * Time.deltaTime * GetRightSpeed(movement.y);
			rotationIncrement += ANGULAR_SPEED_ROTATION_MULTIPLIER * Time.deltaTime * GetAngularSpeedRotation(movement.z);

			positionIncrement += forwardSpeed * initForward + rightSpeed * initRight;
		}

		if (controllerRight.GetHairTriggerDown ()) {
			// TODO Move robot's right arm


		} else {
			// Move robot
			Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerRight.transform));

			float forwardSpeed = FORWARD_SPEED_MULTIPLIER * Time.deltaTime * GetForwardSpeed(movement.x);
			float rightSpeed = RIGHT_SPEED_MULTIPLIER * Time.deltaTime * GetRightSpeed(movement.y);
			rotationIncrement += ANGULAR_SPEED_ROTATION_MULTIPLIER * Time.deltaTime * GetAngularSpeedRotation(movement.z);

			positionIncrement += forwardSpeed * initForward + rightSpeed * initRight;
		}

		// Do the average between the vaue of both controllers if they are both used to move the robot (i.e. no hair-trigger pressed)
		if (!controllerRight.GetHairTriggerDown () && !controllerRight.GetHairTriggerDown ()) {
			positionIncrement = positionIncrement / 2f;
			rotationIncrement = rotationIncrement / 2f;
		}

		cubeTest.transform.position += positionIncrement;
		cubeTest.transform.Rotate (new Vector3 (0, rotationIncrement, 0), Space.World);

	}

	// Return pitch value from -180 to 180 based on set up initial pitch
	Vector3 GetControllerAngles(Transform controllerRot)
	{
		Vector3 forwardZY = Vector3.ProjectOnPlane(controllerRot.forward, initRight).normalized;
		float pitch = Mathf.Sign (Vector3.Dot(Vector3.Cross(forwardZY, initForward), initRight)) * Mathf.Acos (Vector3.Dot (forwardZY, initForward)) * Mathf.Rad2Deg;

		Vector3 forwardXZ = Vector3.ProjectOnPlane(controllerRot.forward, initUp).normalized;
		float yaw = Mathf.Sign (Vector3.Dot(Vector3.Cross(forwardXZ, initForward), initUp)) * Mathf.Acos (Vector3.Dot (forwardXZ, initForward)) * Mathf.Rad2Deg;

		Vector3 upYX = Vector3.ProjectOnPlane(controllerRot.up, initForward).normalized;
		float roll = Mathf.Sign (Vector3.Dot(Vector3.Cross(upYX, initUp), initForward)) * Mathf.Acos (Vector3.Dot (upYX, initUp)) * Mathf.Rad2Deg;

		return new Vector3(pitch, yaw, roll);
	}

	Vector3 ApplyControllerAnglesClamp(Vector3 unClampedAngles)
	{
		return new Vector3(
			Mathf.Max(Mathf.Min(unClampedAngles.x, PITCH_ANGLE_RANGE/2), -PITCH_ANGLE_RANGE/2),
			Mathf.Max(Mathf.Min(unClampedAngles.y, YAW_ANGLE_RANGE/YAW_ANGLE_RANGE), -YAW_ANGLE_RANGE/2),
			Mathf.Max(Mathf.Min(unClampedAngles.z, ROLL_ANGLE_RANGE/2), -ROLL_ANGLE_RANGE/2)
		);
	}

	float GetForwardSpeed(float pitchAngle)
	{
		if (Mathf.Abs(pitchAngle) >= PITCH_NEUTRAL_TOLERANCE) {
			return pitchAngle / (PITCH_ANGLE_RANGE/2f);
		} else {
			return 0f;
		}
	}

	float GetRightSpeed(float yawAngle)
	{
		if (Mathf.Abs(yawAngle) >= YAW_NEUTRAL_TOLERANCE) {
			return yawAngle / (YAW_ANGLE_RANGE/2f);
		} else {
			return 0f;
		}
	}

	float GetAngularSpeedRotation(float rollAngle)
	{
		if (Mathf.Abs(rollAngle) >= ROLL_NEUTRAL_TOLERANCE) {
			return rollAngle / (ROLL_ANGLE_RANGE/2f);
		} else {
			return 0f;
		}
	}

}
