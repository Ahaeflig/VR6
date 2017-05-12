using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

    [SerializeField]
    GameObject controller;

    //private ViveCustomController controllerScript;
    private KeyboardMovement controllerScript;

    Rigidbody rigid; 

    [SerializeField]
    float speedMultiplier = 5;
    [SerializeField]
    float rotationMultiplier = 1f;
    [SerializeField]
    float maxAngularVelocity = 1f;

    // Use this for initialization
    void Start () {
        //TODO replace keyboard with vive when implemented
        //controllerScript = controller.GetComponent<ViveCustomController>();
        controllerScript = controller.GetComponent<KeyboardMovement>();
        rigid = GetComponent<Rigidbody>();
        rigid.maxAngularVelocity = maxAngularVelocity;
    }
	
	void Update () {
        //Add Hover animation


	}

    // Called every physic tick, so more often than Update
    void FixedUpdate()
    {
        //For movement along XZ plane without inertia
        //rigid.velocity = this.transform.forward * speed.x * speedMultiplier + this.transform.right * speed.y * speedMultiplier;
        Vector3 speed = controllerScript.getSpeedVector();
        rigid.AddRelativeForce(new Vector3(speed.y * speedMultiplier, 0f, speed.x * speedMultiplier));
        rigid.AddRelativeTorque(new Vector3(0, speed.z * rotationMultiplier, 0), ForceMode.Acceleration);
    }

}
