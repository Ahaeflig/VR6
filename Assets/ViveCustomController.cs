using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO Turn this class into  a Singleton
public class ViveCustomController : MonoBehaviour {

	[SerializeField]
	GameObject robot;
	private Transform robotsRightShoulder;
	private Transform robotsLeftShoulder;
	private Transform robotsRightHand;
	private Transform robotsLeftHand;
	private float robotArmLength = 0f;

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
	float lastRobotRotation = 0f;

	Vector3 initControllerRightHandPosition = Vector3.zero;
	Vector3 initControllerLeftHandPosition = Vector3.zero;

    float forwardSpeed = 0f;
    float rightSpeed = 0f;
    float angularSpeed = 0f;

    bool haveControllersBeenInitByPlayer = false;

	// Use this for initialization
	void Start () {
		InitControllers ();

		robotsRightShoulder = robot.transform.Find ("R_shoulder");
		robotsRightHand = robot.transform.Find ("R_hand");

		robotArmLength = (robotsRightHand.position - robotsRightShoulder.position).magnitude;
	}

	void InitControllers() {

        haveControllersBeenInitByPlayer = true;

        // Left controller

        int controllerLeftIndex = SteamVR_Controller.GetDeviceIndex (SteamVR_Controller.DeviceRelation.Leftmost);

		if (controllerLeftIndex == (int) controller1.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller1.GetComponent<SteamVR_TrackedObject> ();
		} else if (controllerLeftIndex == (int) controller2.GetComponent<SteamVR_TrackedObject> ().index) {
			trackedControllerLeft = controller2.GetComponent<SteamVR_TrackedObject> ();
		} else {
			trackedControllerLeft = null;
            haveControllersBeenInitByPlayer = false;
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
            haveControllersBeenInitByPlayer = false;
        }

        controllerRight = SteamVR_Controller.Input (controllerRightIndex);
	}

	// Update is called once per frame
	void Update () {

		// Reset the neutral orientation of the controllers if the grip button is clicked
		// Reset the position of the hand of the player with arm fully extended in front of him if the thumbstick is clicked
		if (controllerRight.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			InitControllers ();

			initRight = trackedControllerRight.transform.right;
			initUp = trackedControllerRight.transform.up;
			initForward = trackedControllerRight.transform.forward;
			lastRobotRotation = robot.transform.eulerAngles.y;
		}
        if (controllerLeft.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            InitControllers();

            initRight = trackedControllerLeft.transform.right;
			initUp = trackedControllerLeft.transform.up;
			initForward = trackedControllerLeft.transform.forward;
			lastRobotRotation = robot.transform.eulerAngles.y;
		}

		if (controllerRight.GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) {
			InitControllers ();

			initControllerRightHandPosition = trackedControllerRight.transform.position;
		}
		if (controllerLeft.GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) {
			InitControllers ();

			initControllerLeftHandPosition = trackedControllerLeft.transform.position;
		}

		{
			// Update the init transforms to reflect the rotation of the robot with respect to the last frame

			float newRobotRotation = robot.transform.eulerAngles.y;

			Quaternion robotRotationBetweenFrames = Quaternion.AngleAxis (newRobotRotation - lastRobotRotation, robot.transform.up);

			initRight = robotRotationBetweenFrames * initRight;
			initUp = robotRotationBetweenFrames * initUp;
			initForward = robotRotationBetweenFrames * initForward;

			initControllerRightHandPosition = robotRotationBetweenFrames * trackedControllerRight.transform.position;
			initControllerLeftHandPosition = robotRotationBetweenFrames * trackedControllerLeft.transform.position;

			lastRobotRotation = newRobotRotation;
		}
			
        if (haveControllersBeenInitByPlayer) {

            float tempForwardSpeed = 0f;
            float tempRightSpeed = 0f;
            float tempAngularSpeed = 0f;

            if (controllerLeft.GetHairTrigger ()) {
                // TODO Move robot's left arm


            } else {
                // Move robot
                Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerLeft.transform));

                tempForwardSpeed +=  GetForwardSpeed(movement.x);
                tempRightSpeed += GetRightSpeed(movement.y);
                tempAngularSpeed += GetAngularSpeedRotation(movement.z);
            }

            if (controllerRight.GetHairTrigger ()) {
                // Move robot's right arm

				Vector3 controllerRightHandPosition = trackedControllerRight.transform.position;
				Vector3 playerRightShoulderPosition = playersHead.transform.position;
				Vector3 armVector = controllerRightHandPosition - playerRightShoulderPosition;
				Vector3 rightTargetOrientation = armVector.normalized;
				float lengthRatio = robotArmLength / armVector.magnitude;

				Vector3 newRobotsRightHandTargetPosition = robotsRightShoulder.position + lengthRatio * robotArmLength * rightTargetOrientation;

				robotsRightHandTarget.transform.position.Set (newRobotsRightHandTargetPosition.x, newRobotsRightHandTargetPosition.y, newRobotsRightHandTargetPosition.z);
            } else {
                // Move robot
                Vector3 movement = ApplyControllerAnglesClamp(GetControllerAngles(trackedControllerRight.transform));

                tempForwardSpeed += GetForwardSpeed(movement.x);
                tempRightSpeed += GetRightSpeed(movement.y);
                tempAngularSpeed += GetAngularSpeedRotation(movement.z);
            }

            // Do the average between the vaue of both controllers if they are both used to move the robot (i.e. no hair-trigger pressed)
            if (!controllerRight.GetHairTriggerDown() && !controllerRight.GetHairTriggerDown()) {
                tempForwardSpeed = tempForwardSpeed / 2f;
                tempRightSpeed = tempRightSpeed / 2f;
                tempAngularSpeed = tempAngularSpeed / 2f;
            }

            forwardSpeed = tempForwardSpeed;
            rightSpeed = tempRightSpeed;
            angularSpeed = tempAngularSpeed;
        }
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
