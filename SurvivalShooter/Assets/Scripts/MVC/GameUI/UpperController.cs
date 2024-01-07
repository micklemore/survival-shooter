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
		AddEventListenerToApp(MVCEvent.GameUIEvent.PLAYER_HEALTH_MODIFY, PlayerHealthModifyHandler);
		AddEventListenerToApp(MVCEvent.GameUIEvent.ENEMY_COUNT_MODIFY, UpdateEnemiesCount);
		AddEventListenerToApp(MVCEvent.GameUIEvent.NEW_HORDE_STARTED, UpdateHordeNumber);
		AddEventListenerToApp(MVCEvent.GameUIEvent.AMMUNITION_IN_INVENTORY_UPDATE, UpdateAmmunitionInInventory);
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
	}

	private void UpdateEnemiesCount(object[] data)
	{
		Debug.Log("UpdateEnemiesCount");

		if (upperView)
		{
			int enemiesLeft = (int)data[0];
			int totalEnemies = (int)data[1];

			upperView.UpdateEnemiesLeft(enemiesLeft, totalEnemies);
		}
	}

	private void UpdateHordeNumber(object[] data)
	{
		Debug.Log("UpdateHordeNumber");

		if (upperView)
		{
			int hordeNumber = (int)data[0];
			upperView.UpdateHordeNumber(hordeNumber);
		}
	}

	private void UpdateAmmunitionInInventory(object[] data)
	{
		Debug.Log("UpdateAmmunitionInInventory");
		
		if (upperView)
		{
			Dictionary<int, int> ammunitions = (Dictionary<int, int>)data[0];
			upperView.UpdateInventory(ammunitions);
		}
	}
}