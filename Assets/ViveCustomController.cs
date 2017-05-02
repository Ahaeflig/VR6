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
    Vector3 zeroForward = Vector3.forward;



	// Use this for initialization
	void Start () {
 
    }
   
	// Update is called once per frame
	void Update () {

        print(GetPitch2(controllerRight.transform));

    }


    // Return pitch value from -180 to 180 based on set up initial pitch
    float GetPitch(Transform controllerRot)
    {
        Vector3 ControllerUp = controllerRot.up;
        Vector3 ControllerRight = controllerRot.right;
        Vector3 ControllerForward = controllerRot.forward;

        float forwardPointUp = Vector3.Dot(Vector3.Cross(ControllerUp, Vector3.up), ControllerRight);
        float above = (forwardPointUp > 0) ? 1 : 0;
        bool isAbove = (above == 1) ? true : false;

        Vector3 projectedForward = Vector3.ProjectOnPlane(ControllerForward, Vector3.right);

        if (isAbove)
        {
            return 180 - Mathf.Acos(Vector3.Dot(projectedForward, zeroForward)) * Mathf.Rad2Deg;
        }
        else
        {
            return Mathf.Acos(Vector3.Dot(projectedForward, zeroForward)) * Mathf.Rad2Deg - 180;
        }
    }


    float GetPitch2(Transform controllerRot)
    {
        Vector3 ControllerUp = controllerRot.up;
        Vector3 ControllerRight = controllerRot.right;
        Vector3 ControllerForward = controllerRot.forward;

        Vector3 projectedForward = Vector3.ProjectOnPlane(ControllerForward, Vector3.right);

        projectedForward.Normalize();

        float cosTheta = Mathf.Acos(Vector3.Dot(projectedForward, zeroForward));
        float sinTheta = Mathf.Asin(Vector3.Cross(projectedForward, zeroForward).magnitude);

        print("****");
        print(projectedForward - ControllerForward);
        print(cosTheta * Mathf.Rad2Deg);
        print(sinTheta * Mathf.Rad2Deg);

        float angle = Mathf.Atan2(sinTheta, cosTheta) * Mathf.Rad2Deg;

        return angle;
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

/*
 
        Vector3 currentForward = controllerRight.transform.forward;

        Vector3 upYZ = Vector3.Project(controllerRight.transform.up, Vector3.right);
        Vector3 forwardYZ = Vector3.Project(controllerRight.transform.forward, Vector3.right);


        Vector3 normalYZ = Vector3.Cross(upYZ, forwardYZ);


        //Vector3.Project();
     
        float angleGlobalUp = Vector3.Dot(controllerRight.transform.up, Vector3.up);
        float forwardPointUp = Vector3.Dot(Vector3.Cross(controllerRight.transform.up, Vector3.up), controllerRight.transform.right);
        float above = (forwardPointUp > 0) ? 1 : 0;
        bool isAbove = (above == 1) ? true : false;

        Vector3 projectedForward;
        projectedForward = Vector3.ProjectOnPlane(currentForward, Vector3.right);
        float angle;
                
        if (isAbove)
        {
            angle = 180 - Mathf.Acos(Vector3.Dot(projectedForward, zeroForward)) * Mathf.Rad2Deg;
        }
        else
        {
            angle = Mathf.Acos(Vector3.Dot(projectedForward, zeroForward)) * Mathf.Rad2Deg - 180;
        }


        print(angle);

 
 */






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