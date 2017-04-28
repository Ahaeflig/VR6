using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot : MonoBehaviour {

    [SerializeField]
    GameObject controller;

    private ViveCustomController controllerScript;

    [SerializeField]
    float ForwardSpeedMultiplier = 100f;

	// Use this for initialization
	void Start () {

        controllerScript = controller.GetComponent<ViveCustomController>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        float ForwardSpeed = controllerScript.getControllerFrontSpeed();


        GetComponent<Rigidbody>().velocity = this.transform.forward * ForwardSpeed * ForwardSpeedMultiplier;

    }


}
