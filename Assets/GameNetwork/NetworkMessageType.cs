using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkMessageType {
	public static short GetWallObstacles = MsgType.Highest + 3;
	public static short WallObstacles = MsgType.Highest + 4;
	public static short GetRobotPosition = MsgType.Highest + 5;
	public static short RobotPosition = MsgType.Highest + 6;
	public static short TriggerWallObstacle = MsgType.Highest + 7;
	public static short GetBlocks = MsgType.Highest + 8;
	public static short Block = MsgType.Highest + 9;
	public static short Start = MsgType.Highest + 10;
	public static short GameOver = MsgType.Highest + 11;
	public static short Finish = MsgType.Highest + 12;
	public static short GetPlateforms = MsgType.Highest + 13;
	public static short Plateform = MsgType.Highest + 14;
	public static short GetWallObstaclesImage = MsgType.Highest + 15;
	public static short WallObstaclesImage = MsgType.Highest + 16;
	public static short WallObstacleHasFinished = MsgType.Highest + 17;



}

public class EmptyMessage : MessageBase {

}
	
public class WallObstacleMessage : MessageBase {
	public Vector3 position;
	public Vector3 size;
	public string name;
	public Quaternion rotation;
}

public class RobotPositionMessage : MessageBase{
	public Vector3 position;
}

public class TriggerWallObstacleMessage : MessageBase{
	public string name;
}

public class WallObstacleHasFinishedMessage : MessageBase{
	public string name;
}

public class BlockMessage : MessageBase {
	public Vector3 position;
	public Vector3 size;
	public string name;
	public string materialName;
}

public class PlateformMessage : MessageBase {
	public Vector3 position;
	public Vector3 size;
	public string name;
	public string materialName;
}


