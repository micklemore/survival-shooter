using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MVCEvent
{
	public class GameUIEvent
	{
		public static int PLAYER_HEALTH_MODIFY = 1;
		public static int ENEMY_COUNT_MODIFY = 2;
		public static int NEW_HORDE_STARTED = 3;
		public static int AMMUNITION_IN_INVENTORY_UPDATE = 4;
	}
}
