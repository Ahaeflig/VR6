using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanLine : MonoBehaviour {

	public GameObject miniMap;
	public GameObject robotBlip;
	public float scanSpeed = 200.0f;

	// Use this for initialization
	void Start () {
		var color = robotBlip.transform.GetComponent<Image> ().color;
		robotBlip.transform.GetComponent<Image> ().color = new Color(color.r, color.g, color.b, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		var scanLineY = this.GetComponent<RectTransform> ().localPosition.y;
		var miniMapHeight = this.GetComponentInParent<MiniMap> ().GetComponentInParent<RectTransform> ().rect.height / 2.0f;

		Debug.Log (scanLineY  + " " +robotBlip.transform.localPosition.y);

		if (scanLineY >= (robotBlip.transform.localPosition.y - 20) && scanLineY <= (robotBlip.transform.localPosition.y + 20)) {
			var color = robotBlip.transform.GetComponent<Image> ().color;
			robotBlip.transform.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.9f);
		} else {
			var color = robotBlip.transform.GetComponent<Image> ().color;
			robotBlip.transform.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.0f);
		}

		if (scanLineY > miniMapHeight) {
			this.transform.localPosition = new Vector3(this.transform.localPosition.x, -miniMapHeight, this.transform.localPosition.z);
		} else {
			transform.Translate (0, Time.deltaTime * scanSpeed, 0);
		}
			

	}
}
