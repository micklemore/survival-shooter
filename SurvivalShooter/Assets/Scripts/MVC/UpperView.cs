using SmartMVC;
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
	TextMeshProUGUI numberOfKillsText;

    [SerializeField]
    Slider healthBarValue;

    string killsString = "Kills: ";

    int kills = 0;

    public void UpdateKills()
    {
        Debug.Log("UpdateKills");
        kills++;
        numberOfKillsText.text = killsString + kills;
	}

    public void UpdatePlayerHealth(float actualHealth, float totalHealth)
    {
		Debug.Log("UpdatePlayerHealth: " + actualHealth + "/" + totalHealth);
        healthBarValue.maxValue = totalHealth;
        healthBarValue.value = actualHealth;
        healthBarText.text = actualHealth + "/" + totalHealth;
	}
}
