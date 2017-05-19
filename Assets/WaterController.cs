using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour {

	public float deltaY = 2;
	public float maxY = 10;
	private float totalY = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(totalY) > maxY) {
			deltaY = deltaY * -1;
		}
		transform.Translate(new Vector3(0, deltaY * Time.deltaTime, 0));
		totalY = totalY + deltaY;
	}
}
