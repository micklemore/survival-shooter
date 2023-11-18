using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawn : MonoBehaviour, IPickable
{
    public int Id => id;
    [SerializeField]
    int id;

	public Transform GetGameObjectTransform()
	{
		return transform;
	}
}
