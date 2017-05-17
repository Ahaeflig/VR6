using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovement : MonoBehaviour {

    private float forwardSpeed = 10;
    private float backwardSpeed = 10;
    private float rightSpeed = 10;
    private float leftSpeed = 10;

    private float rotateRight = 10;
    private float rotateLeft = 10;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
        {
            leftSpeed = -1;
        } else {
            leftSpeed = 0;
        }

        if (Input.GetKey(KeyCode.W))
        {
            forwardSpeed = 1;
        } else
        {
            forwardSpeed = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            backwardSpeed = -1; 
        } else
        {
            backwardSpeed = 0f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightSpeed = 1;
        } else
        {
            rightSpeed = 0f;
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            rotateRight = 1;
        } else
        {
            rotateRight = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rotateLeft = 1;
        }
        else
        {
            rotateLeft = 0;
        }

    }

    private void FixedUpdate()
    {
    }

    public Vector3 getSpeedVector()
    {
        return new Vector3(forwardSpeed + backwardSpeed, rightSpeed + leftSpeed, rotateRight - rotateLeft);
    }

}
