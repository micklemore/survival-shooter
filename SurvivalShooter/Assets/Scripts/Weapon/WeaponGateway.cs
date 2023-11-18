using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGateway : MonoBehaviour
{
    public static WeaponGateway instance;

	[SerializeField]
	List<WeaponConstructor> weapons;

	void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
		}
		instance = this;
	}

	public WeaponConstructor GetWeaponById(int weaponId)
	{
		foreach (WeaponConstructor weaponSO in weapons)
		{
			if (weaponSO.Id == weaponId)
			{
				return weaponSO;
			}
		}
		return null;
	}
} 
