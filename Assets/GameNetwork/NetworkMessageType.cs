using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class NetworkMessageType {
	public static short GetTerrain = MsgType.Highest + 1;
	public static short Terrain = MsgType.Highest + 2;
	public static short GetWallObstacles = MsgType.Highest + 3;
	public static short WallObstacles = MsgType.Highest + 4;
	public static short GetRobotPosition = MsgType.Highest + 5;
	public static short RobotPosition = MsgType.Highest + 6;
	public static short TriggerWallObstacle = MsgType.Highest + 7;
};

public class EmptyMessage : MessageBase {

}

public class TerrainMessage : MessageBase
{
	public float width;
	public float height;
}
	
public class WallObstacleMessage : MessageBase
{
	public Vector3 position;
	public Vector3 size;
}

public class RobotPositionMessage : MessageBase
{
	public Vector3 position;
}

