using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

    [SerializeField]
    GameObject controller;

    [SerializeField]
    GameObject controllerViveObject;


    //private ViveCustomController controllerScript;
    private KeyboardMovement controllerKeyboard;
    private ViveCustomController controllerVive;

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
		controllerKeyboard = controller.GetComponent<KeyboardMovement>();
        controllerVive = controllerViveObject.GetComponent<ViveCustomController>();
        rigid = GetComponent<Rigidbody>();
        rigid.maxAngularVelocity = maxAngularVelocity;

		//this.transform.position = HMD.transform.GetChild (2).transform.position;
		//this.transform.rotation =  HMD.transform.GetChild (2).transform.rotation;

    }

	void Update () {
        //Add Hover animation

		//Vector3 speed = controllerVive.getSpeedVector();
		//Vector3 rotate = new Vector3 (0, speed.z * rotationMultiplier, 0) * Time.deltaTime * rotationMultiplier;
		//print (rotate);
		//this.transform.Rotate(rotate);

	}

    // Called every physic tick, so more often than Update
    void FixedUpdate()
    {
        //For movement along XZ plane without inertia
        //rigid.velocity = this.transform.forward * speed.x * speedMultiplier + this.transform.right * speed.y * speedMultiplier;


		Vector3 speed = controllerVive.getSpeedVector();

		//print(speed);

        rigid.AddRelativeForce(new Vector3(speed.y * speedMultiplier, 0f, speed.x * speedMultiplier));
        rigid.AddRelativeTorque(new Vector3(0, speed.z * rotationMultiplier, 0), ForceMode.Acceleration);
  
    }



}
