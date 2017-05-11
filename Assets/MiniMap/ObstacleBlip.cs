using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ObstacleBlip : Blip {

	private bool isTriggered = false;

	void LateUpdate()
	{
		base.LateUpdate ();
		var newBlipSize = map.getMapCoordinateForTarget (target.GetComponent<Renderer> ().bounds.size);
		myRectTransform.GetComponent<RectTransform>().sizeDelta = new Vector2 (newBlipSize.x , newBlipSize.y);
		myRectTransform.localScale = new Vector3 (1, 1, 1);
	}

	public void TriggerObstacle()
	{
		Debug.Log("You have clicked the button!!!");
		//target.transform.localPosition = new Vector3 (target.transform.localPosition.x, -130.0f, target.transform.localPosition.z);
		//var colors = this.GetComponent<Button> ().colors.normalColor;
		//this.GetComponent<Button> ().colors.normalColor = new Color (colors.r, colors.g, colors.b, 0.5f);
		isTriggered = true;
		//initY = target.transform.position.y;
	}

	void Update()
	{
		if (isTriggered) {
			target.transform.Translate (0, 300f * Time.deltaTime,0);
			float maxY = target.GetComponent<Obstacle>().maxY;
			if (target.transform.position.y >= maxY) {
				isTriggered = false;
			}
		}
	}
}
