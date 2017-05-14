﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WallObstacleBlip : MonoBehaviour {

	public ClientNetwork clientNetwork;

	public void Start () 
	{
	}

	public void TriggerObstacle()
	{
		TriggerWallObstacleMessage msg = new TriggerWallObstacleMessage();
		msg.name = this.transform.name;
		clientNetwork.myClient.Send(NetworkMessageType.TriggerWallObstacle, msg);
	}

	void Update()
	{
		
	}

	void LateUpdate () {
	}
}
