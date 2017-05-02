using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO Turn this class into  a Singleton
public class ViveCustomController : MonoBehaviour {

    [SerializeField]
    GameObject controllerRight;
    [SerializeField]
    GameObject controllerLeft;

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
	Vector3 initRight = Vector3.right.Normalize();
	Vector3 initUp = Vector3.up.Normalize();
	Vector3 initForward = Vector3.forward.Normalize();

	// Use this for initialization
	void Start () {
 
    }
   
	// Update is called once per frame
	void Update () {
		
        print(GetControllerAngles(controllerRight.transform));
    
	}
		
    // Return pitch value from -180 to 180 based on set up initial pitch
	float GetControllerAngles(Transform controllerRot)
    {
		Vector3 rightXZ = Vector3.ProjectOnPlane(controllerRot.right, initUp).Normalize();
		Vector3 upYX = Vector3.ProjectOnPlane(controllerRot.up, initForward).Normalize();
		Vector3 forwardZY = Vector3.ProjectOnPlane(controllerRot.forward, initRight).Normalize();

		float pitch = Mathf.Sign (controllerRot.forward.y) * Mathf.Acos (Vector3.Dot (forwardZY, initForward)) * Mathf.Rad2Deg;
		float yaw = Mathf.Sign (controllerRot.right.z) * Mathf.Acos (Vector3.Dot (rightXZ, initRight)) * Mathf.Rad2Deg;
		float roll = Mathf.Sign (controllerRot.up.x) * Mathf.Acos (Vector3.Dot (upYX, initUp)) * Mathf.Rad2Deg;

		return Vector3(pitch, yaw, roll);
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
