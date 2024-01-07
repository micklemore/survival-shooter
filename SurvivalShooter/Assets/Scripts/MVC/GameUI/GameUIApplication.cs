using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIApplication : BaseApplication
{
    public static GameUIApplication instance;

	protected override void Awake()
	{
		base.Awake();
		if (instance != null)
		{
			Destroy(instance);
		}
		instance = this;
	}
}
