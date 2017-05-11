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


		if (scanLineY >= (robotBlip.transform.localPosition.y - 0) && scanLineY <= (robotBlip.transform.localPosition.y + 10)) {
			var color = robotBlip.transform.GetComponent<Image> ().color;
			robotBlip.transform.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, 0.9f);
		} else if (scanLineY > (robotBlip.transform.localPosition.y) && scanLineY < (robotBlip.transform.localPosition.y + 50)) {
			var color = robotBlip.transform.GetComponent<Image> ().color;
			var progress = 1f - (Mathf.Abs (scanLineY - robotBlip.transform.localPosition.y)) / 50.0f;
			robotBlip.transform.GetComponent<Image> ().color = new Color (color.r, color.g, color.b, Mathf.Lerp (0f, 1f, progress));
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
