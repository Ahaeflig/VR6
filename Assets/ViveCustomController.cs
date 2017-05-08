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

    // Not const so that we can change these directly on the inspector
    [SerializeField]
    float PITCH_ANGLE_LOWER_THRESHOLD = 0f;
    [SerializeField]
    float PITCH_ANGLE_ZERO_THRESHOLD = 65f;    
    [SerializeField]
    float PITCH_ANGLE_UPPER_THRESHOLD = 130f;

    private float lastForwardSpeed = 0f;

    //TODO make it change with button pressed
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

		print(GetControllerAngles(trackedControllerRight.transform));
    
	}

    // Return pitch value from -180 to 180 based on set up initial pitch
    Vector3 GetControllerAngles(Transform controllerRot)
    {
		Vector3 forwardXZ = Vector3.ProjectOnPlane(controllerRot.forward, initUp).normalized;
		Vector3 upYX = Vector3.ProjectOnPlane(controllerRot.up, initForward).normalized;
		Vector3 forwardZY = Vector3.ProjectOnPlane(controllerRot.forward, initRight).normalized;

		float pitch = Mathf.Sign (controllerRot.forward.y) * Mathf.Acos (Vector3.Dot (forwardZY, initForward)) * Mathf.Rad2Deg;
		float yaw = Mathf.Sign (controllerRot.forward.x) * Mathf.Acos (Vector3.Dot (forwardXZ, initForward)) * Mathf.Rad2Deg;
		float roll = Mathf.Sign (controllerRot.up.x) * Mathf.Acos (Vector3.Dot (upYX, initUp)) * Mathf.Rad2Deg;

		return new Vector3(pitch , yaw, roll);
    }
		
    //return val between [-1,1] use with rigidbody.velocity probably
    // thresholds the pitch angle to return speed TODO fix XAVIER
    float GetForwardSpeed(float pitch360)
    {
        //TODO Maybe we can change GetPitchZeroTo360 to return between [180 , -180] directly
        if (pitch360 > 180)
            pitch360 = pitch360 - 360;
        
        //Now use threshold to get a speed scale
        if (pitch360 < PITCH_ANGLE_LOWER_THRESHOLD)
            return lastForwardSpeed;
        else if (pitch360 > PITCH_ANGLE_UPPER_THRESHOLD)
            return lastForwardSpeed;
        else {
            lastForwardSpeed = (PITCH_ANGLE_ZERO_THRESHOLD - pitch360) * 2 / (PITCH_ANGLE_UPPER_THRESHOLD - PITCH_ANGLE_LOWER_THRESHOLD);
            return lastForwardSpeed;
        }
    }

}
