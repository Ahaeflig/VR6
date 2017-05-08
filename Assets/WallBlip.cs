using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBlip : Blip {

	void LateUpdate()
	{
		base.LateUpdate ();
		Debug.Log (target.GetComponent<Renderer> ().bounds.size.x);
		var newBlipSize = map.getMapCoordinateForTarget (target.GetComponent<Renderer> ().bounds.size);
		//myRectTransform.localScale = new Vector3 newBlipSize.x , newBlipSize.y * 0.001f,  target.transform.localScale.z);
	}
}
