using SmartMVC;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpperView : View
{
    [SerializeField]
	TextMeshProUGUI healthBarText;

    [SerializeField]
    Slider healthBarValue;

    [SerializeField]
    TextMeshProUGUI hordeNumberText;

    [SerializeField]
    TextMeshProUGUI enemiesLeftText;

    [SerializeField]
    TextMeshProUGUI projectilesInCurrentWeaponText;

    [SerializeField]
    TextMeshProUGUI inventoryText;

    public void UpdatePlayerHealth(float actualHealth, float totalHealth)
    {
		Debug.Log("UpdatePlayerHealth: " + actualHealth + "/" + totalHealth);
        healthBarValue.maxValue = totalHealth;
        healthBarValue.value = actualHealth;
        healthBarText.text = actualHealth + "/" + totalHealth;
	}

    public void UpdateEnemiesLeft(int enemiesLeft, int totalEnemies)
    {
        enemiesLeftText.text = "Enemies left: " + enemiesLeft + "/" + totalEnemies;
    }

	public void UpdateHordeNumber(int hordeNumber)
	{
        hordeNumberText.text = "Horde: " + hordeNumber;
	}

    public void UpdateInventory(Dictionary<int, int> inventory)
    {
        string ammunitions = "";
        foreach (int weaponId  in inventory.Keys)
        {
            ammunitions += weaponId + ": " + inventory[weaponId] + "        ";
        }
        inventoryText.text = ammunitions;
    }
}
