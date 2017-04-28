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

	// Use this for initialization
	void Start () {

	}

    
    public float getControllerFrontSpeed()
    {
        return GetForwardSpeed(GetPitchZeroTo360(controllerRight.transform.up, controllerRight.transform.right));
    }
	
	// Update is called once per frame
	void Update () {

        //print(getControllerFrontSpeed());

        //print(GetYawZeroTo360(controllerRight.transform.forward, controllerRight.transform.up));


        float a = Vector3.Dot(controllerRight.transform.right, Vector3.forward);
        //print(Mathf.Rad2Deg * a);

        //print(controllerRight.transform.rotation.eulerAngles.y);
        //print(controllerRight.transform.right);

        //print(Vector3.Cross(controllerRight.transform.forward, Vector3.forward));
        //print(GetPitchZeroTo360(controllerRight.transform.right, controllerRight.transform.forward));

        print(Vector3.Dot(controllerRight.transform.right, Vector3.right) * Mathf.Rad2Deg);
    }


    // Return yaw value from 0 to 360 
    float GetYawZeroTo360(Vector3 ControllerForward, Vector3 ControllerUp)
    {

        Vector3.Cross(ControllerForward, ControllerUp);



        return 0f;

    }

    //return val between [-1,1] use with rigidbody.velocity probably
    // thresholds the pitch angle to return speed TODO fix XAVIER
    float GetLateralSpeed(float yaw360)
    {
        //TODO Maybe we can change GetPitchZeroTo360 to return between [180 , -180] directly
        if (yaw360 > 180)
            yaw360 = yaw360 - 360;

        //Now use threshold to get a speed scale
        if (yaw360 < PITCH_ANGLE_LOWER_THRESHOLD)
            return lastForwardSpeed;
        else if (yaw360 > PITCH_ANGLE_UPPER_THRESHOLD)
            return lastForwardSpeed;
        else
        {
            lastForwardSpeed = (PITCH_ANGLE_ZERO_THRESHOLD - yaw360) * 2 / (PITCH_ANGLE_UPPER_THRESHOLD - PITCH_ANGLE_LOWER_THRESHOLD);
            return lastForwardSpeed;
        }
    }



    // Return pitch value from 0 to 360 where 0 =controller "flat" on the ground and 180 = controller upside down, etc...
    float GetPitchZeroTo360(Vector3 ControllerUp, Vector3 ControllerRight)
    {
        float angleGlobalUp = Vector3.Dot(ControllerUp, Vector3.up);
        float forwardPointUp = Vector3.Dot(Vector3.Cross(ControllerUp, Vector3.up), ControllerRight);
        float above = (forwardPointUp > 0) ? 1:0;
        bool isAbove = (above == 1) ? true : false;

        //Maybe we can factor this into one statement
        if (isAbove) {
            //angleGlobalUp va de 1 à -1 et above = 1
            //Je veux mapper de 1 -> 0° à -1 -> -180°
            return (1 - angleGlobalUp) * 90;
            
        } else {
            //angleGlobalUp va de -1 à 1 et above = 0
            //Je veux mapper de -1 -> 180° à -1 -> 360°
            return (1 + angleGlobalUp) * 90 + 180 ;
        }
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








/*float GetPitch(Quaternion rotation)
{

    if (rotation.y > 0 && rotation.x < 0) {
        return 90 - rotation.x - 55;
    }
    else if (rotation.y < 0 && rotation.x < 0 )
    {
        if (rotation.x < -55 && rotation.x > -90)
            return rotation.x + 55;
        else
            return -55 - rotation.x;
    }
    else if (rotation.y < 0 && rotation.x > 0) 
    {
        return 55 + rotation.x ;
    }
    else
    {
        return 9999;
    }


}*/