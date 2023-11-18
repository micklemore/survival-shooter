using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/WeaponConstructor", order = 1)]
public class WeaponConstructor : ScriptableObject
{
	public Weapon Weapon => weapon;
	[SerializeField]
	Weapon weapon;

	public int Id => id;
	[SerializeField]
	int id;

	public float Damage => damage;
	[SerializeField]
    float damage;

	public float ProjectileSpeed => projectileSpeed;
	[SerializeField]
	float projectileSpeed;

	public float RangeDistance => rangeDistance;
	[SerializeField]
	float rangeDistance;

	public float FireSpeed => shootSpeed;
	[SerializeField]
	float shootSpeed;

	public float NumberOfProjectileToShoot => numberOfProjectileToShoot;
	[SerializeField]
	float numberOfProjectileToShoot;

	public float Spread => spread;
	[SerializeField]
	float spread;

	public int ChargerCapacity => chargerCapacity;
	[SerializeField]
	int chargerCapacity;

	public float PushbackForce => pushbackForce;
	[SerializeField]
	float pushbackForce;

	public float TimerUntilNextPushback => timerUntilNextPushback;
	[SerializeField]
	float timerUntilNextPushback;
}
