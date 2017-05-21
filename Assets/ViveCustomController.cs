using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Turn this class into  a Singleton
public class ViveCustomController : MonoBehaviour {

	[SerializeField]
	GameObject robot;
	[SerializeField]
	GameObject robotRightHand;
	[SerializeField]
	GameObject robotLeftHand;

	[SerializeField]
	GameObject robotRightHandTarget;
	[SerializeField]
	GameObject robotLeftHandTarget;

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

	Vector3 initRobotRightHandPosition;
	Vector3 initRobotRightShoulderPosition;
	Vector3 initRobotRightArm;

	Vector3 initRobotLeftHandPosition;
	Vector3 initRobotLeftShoulderPosition;
	Vector3 initRobotLeftArm;

	Vector3 initPlayerRightHandLocalPosition;
	Vector3 initPlayerRightShoulderLocalPosition;
	Vector3 initPlayerRightArm;

	Vector3 initPlayerLeftHandLocalPosition;
	Vector3 initPlayerLeftShoulderLocalPosition;
	Vector3 initPlayerLeftArm;

	Vector3 lastRobotForward = Vector3.zero;

    float forwardSpeed = 0f;
    float rightSpeed = 0f;
    float angularSpeed = 0f;

	bool movementCalibrated = false;

	// Use this for initialization
	void Start () {
		InitControllers ();

		initRobotRightArm = computeArm (robotRightHand.transform, ref initRobotRightHandPosition, ref initRobotRightShoulderPosition, Space.World);
		initRobotLeftArm = computeArm (robotLeftHand.transform, ref initRobotLeftHandPosition, ref initRobotLeftShoulderPosition, Space.World);

		lastRobotForward = robot.transform.forward;
	}

	void InitControllers() {

        // Left controller

        int controllerLeftIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);

		if (controllerLeftIndex == (int) controller1.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller1.GetComponent<SteamVR_TrackedObject> ();
		} else if (controllerLeftIndex == (int) controller2.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller2.GetComponent<SteamVR_TrackedObject> ();
		} else {
			trackedControllerLeft = null;
        }

        controllerLeft = SteamVR_Controller.Input(controllerLeftIndex);

        // Right controller

        int controllerRightIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Rightmost);

		if (controllerRightIndex == (int) controller1.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerRight = controller1.GetComponent<SteamVR_TrackedObject> ();
		} else if (controllerRightIndex == (int) controller2.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerRight = controller2.GetComponent<SteamVR_TrackedObject> ();
		} else {
            trackedControllerRight = null;
        }

        controllerRight = SteamVR_Controller.Input (controllerRightIndex);
	}

	// Update is called once per frame
	void Update () {

		// ********************** CALIBRATION ********************** \\

		// Reset the neutral orientation of the controllers if the grip button is clicked
		// Reset the position of the hand of the player with arm fully extended in front of him if the pad is clicked in the center
		if (controllerRight.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			InitControllers ();

			initRight = trackedControllerRight.transform.right;
			initUp = trackedControllerRight.transform.up;
			initForward = trackedControllerRight.transform.forward;
			lastRobotForward = robot.transform.forward;

			movementCalibrated = true;
		}
		if (controllerLeft.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			InitControllers ();

			initRight = trackedControllerLeft.transform.right;
			initUp = trackedControllerLeft.transform.up;
			initForward = trackedControllerLeft.transform.forward;
			lastRobotForward = robot.transform.forward;

			movementCalibrated = true;
		}

		// ********************** ROBOT'S ROTATION & TRANSLATION UPDATE (since last frame) ********************** \\

		if (movementCalibrated) {
			Vector3 newRobotForward = robot.transform.forward;
			Quaternion robotRotationBetweenFrames = Quaternion.FromToRotation (lastRobotForward, newRobotForward);

			initRight = robotRotationBetweenFrames * initRight;
			initUp = robotRotationBetweenFrames * initUp;
			initForward = robotRotationBetweenFrames * initForward;

			initRobotRightArm = computeArm (robotRightHand.transform, ref initRobotRightHandPosition, ref initRobotRightShoulderPosition, Space.World);
			initRobotLeftArm = computeArm (robotLeftHand.transform, ref initRobotLeftHandPosition, ref initRobotLeftShoulderPosition, Space.World);

			lastRobotForward = newRobotForward;
		}

		// ********************** ROBOT MOVEMENT ********************** \\

		if (movementCalibrated) {

            float tempForwardSpeed = 0f;
            float tempRightSpeed = 0f;
            float tempAngularSpeed = 0f;

            if (!controllerLeft.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
                Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerLeft.transform));

                tempForwardSpeed +=  GetForwardSpeed(movement.x);
                tempRightSpeed += GetRightSpeed(movement.y);
                tempAngularSpeed += GetAngularSpeedRotation(movement.z);
            }

            if (!controllerRight.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
                Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerRight.transform));

                tempForwardSpeed += GetForwardSpeed(movement.x);
                tempRightSpeed += GetRightSpeed(movement.y);
                tempAngularSpeed += GetAngularSpeedRotation(movement.z);
            }

            // Do the average between the value of both controllers if they are both used to move the robot (i.e. no hair-trigger pressed)
            if (!controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                tempForwardSpeed = tempForwardSpeed / 2f;
                tempRightSpeed = tempRightSpeed / 2f;
                tempAngularSpeed = tempAngularSpeed / 2f;
            }

            forwardSpeed = tempForwardSpeed;
            rightSpeed = tempRightSpeed;
            angularSpeed = tempAngularSpeed;
        }

		// ********************** ROBOT ARMS' MOVEMENT ********************** \\

		if (movementCalibrated && controllerRight.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			initPlayerRightArm = computeArm (trackedControllerRight.transform, ref initPlayerRightHandLocalPosition, ref initPlayerRightShoulderLocalPosition, Space.Self);
		} else if (controllerLeft.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			Vector3 currentPlayerLeftHandLocalPosition = trackedControllerLeft.transform.localPosition;
			Vector3 currentPlayerLeftArm = currentPlayerLeftHandLocalPosition - initPlayerLeftShoulderLocalPosition;

			Quaternion playerLeftArmRotationBasedOnInit = Quaternion.FromToRotation (initPlayerLeftArm, currentPlayerLeftArm);
			float playerLeftArmRatioBasedOnInit = currentPlayerLeftArm.magnitude / initPlayerLeftArm.magnitude;

			Vector3 newRobotLeftArm = playerLeftArmRatioBasedOnInit * (playerLeftArmRotationBasedOnInit * initRobotLeftArm);

			robotLeftHandTarget.transform.position = initRobotLeftShoulderPosition + newRobotLeftArm;
		}

		if (movementCalibrated && controllerLeft.GetPressDown (SteamVR_Controller.ButtonMask.Trigger)) {
			initPlayerLeftArm = computeArm (trackedControllerLeft.transform, ref initPlayerLeftHandLocalPosition, ref initPlayerLeftShoulderLocalPosition, Space.Self);
		} else if (controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
			Vector3 currentPlayerRightHandLocalPosition = trackedControllerRight.transform.localPosition;
			Vector3 currentPlayerRightArm = currentPlayerRightHandLocalPosition - initPlayerRightShoulderLocalPosition;

			Quaternion playerRightArmRotationBasedOnInit = Quaternion.FromToRotation (initPlayerRightArm, currentPlayerRightArm);
			float playerRightArmRatioBasedOnInit = currentPlayerRightArm.magnitude / initPlayerRightArm.magnitude;

			Vector3 newRobotRightArm = playerRightArmRatioBasedOnInit * (playerRightArmRotationBasedOnInit * initRobotRightArm);

			robotRightHandTarget.transform.position = initRobotRightShoulderPosition + newRobotRightArm;
		}
	}

	Vector3 computeArm (Transform handTransform, ref Vector3 handPosition, ref Vector3 shoulderPosition, Space context) {
		Vector3 handLocalPosition = handTransform.localPosition;
		Vector3 handWorldPosition = handTransform.position;

		if (context == Space.Self) {
			handPosition = handLocalPosition;
			shoulderPosition = new Vector3 (handLocalPosition.x, handLocalPosition.y, 0);
		} else if (context == Space.World) {
			handPosition = handWorldPosition;
			shoulderPosition = handTransform.parent.TransformPoint(new Vector3 (handLocalPosition.x, handLocalPosition.y, 0));
		}

		return handPosition - shoulderPosition;
	}

    public Vector3 getSpeedVector() {
		return new Vector3(forwardSpeed, rightSpeed, angularSpeed);
    }

    // Return pitch value from -180 to 180 based on set up initial pitch
    Vector3 GetControllerAngles(Transform controllerRot) {
		Vector3 forwardZY = Vector3.ProjectOnPlane(controllerRot.forward, initRight).normalized;
		float pitch = - Mathf.Sign (Vector3.Dot(Vector3.Cross(forwardZY, initForward), initRight)) * Mathf.Acos (Vector3.Dot (forwardZY, initForward)) * Mathf.Rad2Deg;

		Vector3 forwardXZ = Vector3.ProjectOnPlane(controllerRot.forward, initUp).normalized;
		float yaw = - Mathf.Sign (Vector3.Dot(Vector3.Cross(forwardXZ, initForward), initUp)) * Mathf.Acos (Vector3.Dot (forwardXZ, initForward)) * Mathf.Rad2Deg;

		Vector3 upYX = Vector3.ProjectOnPlane(controllerRot.up, initForward).normalized;
		float roll = - Mathf.Sign (Vector3.Dot(Vector3.Cross(upYX, initUp), initForward)) * Mathf.Acos (Vector3.Dot (upYX, initUp)) * Mathf.Rad2Deg;

        return new Vector3(pitch, yaw, roll);
	}

	Vector3 ApplyControllerAnglesClamp(Vector3 unClampedAngles) {
        return new Vector3(
			Mathf.Max(Mathf.Min(unClampedAngles.x, PITCH_ANGLE_RANGE), -PITCH_ANGLE_RANGE),
			Mathf.Max(Mathf.Min(unClampedAngles.y, YAW_ANGLE_RANGE), -YAW_ANGLE_RANGE),
			Mathf.Max(Mathf.Min(unClampedAngles.z, ROLL_ANGLE_RANGE), -ROLL_ANGLE_RANGE)
		);
	}

	float GetForwardSpeed(float pitchAngle) {
		if (Mathf.Abs(pitchAngle) >= PITCH_NEUTRAL_TOLERANCE) {
			return pitchAngle / PITCH_ANGLE_RANGE;
		} else {
			return 0f;
		}
	}

	float GetRightSpeed(float yawAngle) {
		if (Mathf.Abs(yawAngle) >= YAW_NEUTRAL_TOLERANCE) {
			return yawAngle / YAW_ANGLE_RANGE;
		} else {
			return 0f;
		}
	}

	float GetAngularSpeedRotation(float rollAngle) {
		if (Mathf.Abs(rollAngle) >= ROLL_NEUTRAL_TOLERANCE) {
			return rollAngle / ROLL_ANGLE_RANGE;
		} else {
			return 0f;
		}
	}

}
