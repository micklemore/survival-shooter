using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammunition : MonoBehaviour, IPickable
{
	public int WeaponIdCorrespondingToThisAmmo => weaponIdCorrespondingToThisAmmo;
	[SerializeField]
	int weaponIdCorrespondingToThisAmmo;

	public int Amount => amount;
	[SerializeField]
	int amount;

	public Transform GetGameObjectTransform()
	{
		return transform;
	}
}
