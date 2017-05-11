using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlip : Blip {

	void LateUpdate()
	{
		base.LateUpdate ();
		var newBlipSize = map.getMapCoordinateForTarget (target.GetComponent<Renderer> ().bounds.size);
		myRectTransform.GetComponent<RectTransform>().sizeDelta = new Vector2 (newBlipSize.x , newBlipSize.y);
		myRectTransform.localScale = new Vector3 (1, 1, 1);
	}
}
