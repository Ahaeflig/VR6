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
	GameObject robotsRightHandTarget;

	[SerializeField]
	GameObject playersHead;

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

	Vector3 initRobotRightArm;
	Vector3 initRobotLeftArm;

	Vector3 initPlayerRightHandPosition;
	Vector3 initPlayerRightShoulderPosition;
	Vector3 initPlayerLeftHandPosition;
	Vector3 initPlayerLeftShoulderPosition;

	float lastRobotRotation = 0f;
	Vector3 lastRobotPosition = Vector3.zero;

    float forwardSpeed = 0f;
    float rightSpeed = 0f;
    float angularSpeed = 0f;

	bool movementCalibrated = false;
	bool rightArmCalibrated = false;
	bool leftArmCalibrated = false;

	// Use this for initialization
	void Start () {
		InitControllers ();

		Vector3 initRobotRightHandPosition = robotRightHand.transform.localPosition;
		Vector3 initRobotRightShoulderPosition = new Vector3 (initRobotRightHandPosition.x, initRobotRightHandPosition.y, 0);
		initRobotRightArm = robot.transform.TransformPoint(initRobotRightHandPosition) - robot.transform.TransformPoint(initRobotRightShoulderPosition);
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

		// Reset the neutral orientation of the controllers if the grip button is clicked
		// Reset the position of the hand of the player with arm fully extended in front of him if the pad is clicked in the center
		if (controllerRight.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			InitControllers ();

			initRight = trackedControllerRight.transform.right;
			initUp = trackedControllerRight.transform.up;
			initForward = trackedControllerRight.transform.forward;
			lastRobotRotation = robot.transform.eulerAngles.y;

			movementCalibrated = true;
		}
        if (controllerLeft.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            InitControllers();

            initRight = trackedControllerLeft.transform.right;
			initUp = trackedControllerLeft.transform.up;
			initForward = trackedControllerLeft.transform.forward;
			lastRobotRotation = robot.transform.eulerAngles.y;

			movementCalibrated = true;
		}

		if (movementCalibrated && controllerRight.GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) {

			Vector3 initPlayerRightHandLocalPosition = trackedControllerRight.transform.localPosition;
			Vector3 initPlayerRightShoulderLocalPosition = new Vector3 (initPlayerRightHandLocalPosition.x, initPlayerRightHandLocalPosition.y, 0);

			Transform parentTransform = trackedControllerRight.transform.parent;
			initPlayerRightHandPosition = parentTransform.TransformPoint (initPlayerRightHandLocalPosition);
			initPlayerRightShoulderPosition = parentTransform.TransformPoint (initPlayerRightShoulderLocalPosition);

			lastRobotPosition = robot.transform.position;
		
			rightArmCalibrated = true;
		}
		if (movementCalibrated && controllerLeft.GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) {
			
		}

		// Update the init vectors to reflect the rotation of the robot since the last frame
		if (movementCalibrated) {
			float newRobotRotation = robot.transform.eulerAngles.y;
			Vector3 newRobotPosition = robot.transform.position;

			Quaternion robotRotationBetweenFrames = Quaternion.Euler (0, newRobotRotation - lastRobotRotation, 0);
			initRight = robotRotationBetweenFrames * initRight;
			initUp = robotRotationBetweenFrames * initUp;
			initForward = robotRotationBetweenFrames * initForward;

			//Vector3 robotTranslationBetweenFrames = newRobotPosition - lastRobotPosition;
			//initRobotRightArm = initRobotRightArm + robotTranslationBetweenFrames;
			initRobotRightArm = initRobotRightArm;

			lastRobotRotation = newRobotRotation;
			lastRobotPosition = newRobotPosition;
		}

		// Move robot
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

            // Do the average between the vaue of both controllers if they are both used to move the robot (i.e. no hair-trigger pressed)
            if (!controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger) && !controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {
                tempForwardSpeed = tempForwardSpeed / 2f;
                tempRightSpeed = tempRightSpeed / 2f;
                tempAngularSpeed = tempAngularSpeed / 2f;
            }

            forwardSpeed = tempForwardSpeed;
            rightSpeed = tempRightSpeed;
            angularSpeed = tempAngularSpeed;
        }

		// Move robot's arms
		if (leftArmCalibrated && controllerLeft.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			
		}
		if (rightArmCalibrated && controllerRight.GetPress(SteamVR_Controller.ButtonMask.Trigger)) {

			Vector3 currentPlayerRightHandPosition = trackedControllerRight.transform.position;
							
			Vector3 initPlayerRightArm = initPlayerRightHandPosition - initPlayerRightShoulderPosition;
			Vector3 currentPlayerRightArm = currentPlayerRightHandPosition - initPlayerRightShoulderPosition;

			Quaternion playerRightArmRotationBasedOnInit = Quaternion.FromToRotation (initPlayerRightArm, currentPlayerRightArm);
			float playerRightArmRatioBasedOnInit = currentPlayerRightArm.magnitude / initPlayerRightArm.magnitude;

			Vector3 newRobotRightArm = playerRightArmRatioBasedOnInit * (playerRightArmRotationBasedOnInit * initRobotRightArm);

			robotsRightHandTarget.transform.localPosition = initPlayerRightShoulderPosition + newRobotRightArm;

			Debug.DrawLine(initPlayerRightShoulderPosition, initPlayerRightShoulderPosition + newRobotRightArm, Color.red);
		}
	}

    public Vector3 getSpeedVector() {
		//return new Vector3(forwardSpeed, rightSpeed, angularSpeed);
		return Vector3.zero;
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
