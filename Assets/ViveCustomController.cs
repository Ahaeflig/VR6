using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Turn this class into  a Singleton
public class ViveCustomController : MonoBehaviour {

	[SerializeField]
	GameObject controller1;
	[SerializeField]
	GameObject controller2;

	SteamVR_TrackedObject trackedControllerLeft {
		get {
			int controllerLeftIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);

			if (controllerLeftIndex == (int)controller1.GetComponent<SteamVR_TrackedObject> ().index) {
				return controller1.GetComponent<SteamVR_TrackedObject> ();
			} else if (controllerLeftIndex == (int)controller2.GetComponent<SteamVR_TrackedObject> ().index) {
				return controller2.GetComponent<SteamVR_TrackedObject> ();
			} else {
				return null;
			}
		}
	}
	SteamVR_TrackedObject trackedControllerRight {
		get {
			int controllerRightIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);

			if (controllerRightIndex == (int)controller1.GetComponent<SteamVR_TrackedObject> ().index) {
				return controller1.GetComponent<SteamVR_TrackedObject> ();
			} else if (controllerRightIndex == (int)controller2.GetComponent<SteamVR_TrackedObject> ().index) {
				return controller2.GetComponent<SteamVR_TrackedObject> ();
			} else {
				return null;
			}
		}
	}

	SteamVR_Controller.Device controllerLeft { get { return SteamVR_Controller.Input ((int)trackedControllerLeft.index); }}
	SteamVR_Controller.Device controllerRight { get { return SteamVR_Controller.Input ((int)trackedControllerRight.index); }}

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

	Vector3 initRight = Vector3.right;
	Vector3 initUp = Vector3.up;
	Vector3 initForward = Vector3.forward;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		// Reset the initial position of the player if the grip button is clicked
		if (controllerRight.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
			initRight = trackedControllerRight.transform.right;
			initUp = trackedControllerRight.transform.up;
			initForward = trackedControllerRight.transform.forward;
		}

		Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerRight.transform));

		float forwardSpeed = GetForwardSpeed(movement.x);
		float rightSpeed = GetRightSpeed(movement.y);
		float rotationAngle = GetRotationAngle(movement.z);

		print(new Vector3(forwardSpeed, rightSpeed, rotationAngle));

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

	float GetRotationAngle(float rollAngle)
	{
		if (Mathf.Abs(rollAngle) >= ROLL_NEUTRAL_TOLERANCE) {
			return 180f * rollAngle / (ROLL_ANGLE_RANGE/2f);
		} else {
			return 0f;
		}
	}

}
