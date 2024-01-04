using SmartMVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpperController : Controller
{
	UpperView upperView;

	void Awake()
	{
		upperView = Find<UpperView>(null, true);
		AddEventListenerToApp((int)EventsEnum.PLAYER_HEALTH_MODIFY, PlayerHealthModifyHandler);
	}

	void Start()
	{
		

		AddEventListenerToApp((int)EventsEnum.ENEMY_DEATH, EnemyDeathHandler);
	}

	private void EnemyDeathHandler(object[] data)
	{
		Debug.Log("EnemyDeathHandler");
		if(upperView)
		{
			upperView.UpdateKills();
		}
	}

	private void PlayerHealthModifyHandler(object[] data)
	{
		Debug.Log("PlayerHealthModifyHandler");
		if (upperView)
		{
			float actualHealth = (float)data[0];
			float totalHealth = (float)data[1];

			upperView.UpdatePlayerHealth(actualHealth, totalHealth);
		}
		else
		{
			Debug.Log("Upperview null");
		}
	}
}