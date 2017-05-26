using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour {

	private float deltaX = 3f;
	private float deltaY = 1f;
	private float totalX = 0;
	private float maxX = 600; 
	private float totalY = 0;
	private float maxY = 10; 

	// Use this for initialization
	void Start () {
		totalX = Random.Range (0, maxX);
		totalY = Random.Range (0, maxY);
	}
	
	// Update is called once per frame
	void Update () {
		if (Mathf.Abs(totalY) > maxY) {
			totalY = 0;
			deltaY = deltaY * -1;
		}
		if (Mathf.Abs(totalX) > maxX) {
			totalX = 0;
			deltaX = deltaX * -1;
		}
		transform.Translate (deltaX * Time.deltaTime, deltaY * Time.deltaTime , 0);
		totalX = totalX + (deltaX * Time.deltaTime);
		totalY = totalY + (deltaY * Time.deltaTime);
		//transform.position = new Vector3(x , amplitude * Mathf.Sin(x), transform.position.z);
	}
}
