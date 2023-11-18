using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	Transform projectileSpawnSocketTransform;

	[SerializeField]
	Projectile projectile;

	[SerializeField]
	IDamager damager;

	public int Id => id;
	int id;

	Transform weaponSocketRight;

	Transform weaponSocketLeft;

	public bool HasInfiniteCharger => chargerCapacity == 0;

	float fireSpeed;

	float spread;

	float numberOfProjectileToShoot;

	protected float damage;

	float projectileSpeed;

	float rangeDistance;

	int chargerCapacity;

	int numberOfProjectilesInCharger;

	protected float pushbackForce;

	protected float timerUntilNextPushback;

	public Player Owner => owner;
	Player owner;

	float timerUntilNextShoot = 0;

	void Update()
	{
		WaitUntilNextFire();
	}

	public void Equip(Transform weaponSocketRight, Transform weaponSocketLeft, WeaponConstructor weaponConstructor, Player owner)
    {
		InitializeWeaponAttributes(weaponSocketRight, weaponSocketLeft, weaponConstructor, owner);
		AttachWeaponToSocket(weaponSocketRight);
	}

	void InitializeWeaponAttributes(Transform weaponSocketRight, Transform weaponSocketLeft, WeaponConstructor weaponConstructor, Player weaponOwner)
	{
		id = weaponConstructor.Id;
		owner = weaponOwner;
		this.weaponSocketLeft = weaponSocketLeft;
		this.weaponSocketRight = weaponSocketRight;
		fireSpeed = weaponConstructor.FireSpeed; 
		spread = weaponConstructor.Spread;
		numberOfProjectileToShoot = weaponConstructor.NumberOfProjectileToShoot;
		damage = weaponConstructor.Damage;
		projectileSpeed = weaponConstructor.ProjectileSpeed;
		rangeDistance = weaponConstructor.RangeDistance;
		chargerCapacity = weaponConstructor.ChargerCapacity;
		numberOfProjectilesInCharger = chargerCapacity;
		pushbackForce = weaponConstructor.PushbackForce;
		timerUntilNextPushback = weaponConstructor.TimerUntilNextPushback;
	}

	void AttachWeaponToSocket(Transform weaponSocket)
	{
		transform.SetParent(weaponSocket);
	}

	public void SwithWeaponSocket()
	{
		if (transform.position == weaponSocketRight.position)
		{
			transform.position = weaponSocketLeft.position;
			transform.SetParent(weaponSocketLeft);
		}
		else
		{
			transform.position = weaponSocketRight.position;
			transform.SetParent(weaponSocketRight);
		}
	}

	public void Shoot(FactionEnum faction)
	{
		if (CanShoot())
		{
			timerUntilNextShoot = fireSpeed;

			SpawnProjectilesOrMelee(faction);
		}
	}

	public virtual void SpawnProjectilesOrMelee(FactionEnum faction)
	{
		for (int i = 0; i < numberOfProjectileToShoot; i++)
		{
			Projectile projectileToSpawn = Instantiate(projectile, projectileSpawnSocketTransform.position, transform.rotation);
			Vector3 spreadVector = new Vector3(UnityEngine.Random.Range(-spread, spread), UnityEngine.Random.Range(-spread, spread), 0);
			projectileToSpawn.FireProjectile(this, GetDirectionOfFire() + spreadVector, damage, projectileSpeed, rangeDistance, faction, pushbackForce, timerUntilNextPushback);
		}
		numberOfProjectilesInCharger--;
		//fireNotify()
		Debug.Log("projectiles left: " + numberOfProjectilesInCharger);
	}

	bool CanShoot()
	{
		return timerUntilNextShoot <= 0 && (HasInfiniteCharger || numberOfProjectilesInCharger > 0);
	}

	protected Vector3 GetDirectionOfFire()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
	}

	void WaitUntilNextFire()
	{
		if (timerUntilNextShoot > 0)
		{
			timerUntilNextShoot -= Time.deltaTime;
		}
	}

	public int Reload(int amount)
	{
		if (IsChargerFull())
		{
			Debug.Log("charger is full");
		}

		int reloadedProjectiles = 0;

		if (chargerCapacity > numberOfProjectilesInCharger + amount)
		{
			reloadedProjectiles = amount;
		}
		else
		{
			reloadedProjectiles = chargerCapacity - numberOfProjectilesInCharger;
		}

		numberOfProjectilesInCharger += reloadedProjectiles;
		
		Debug.Log("recharged " + reloadedProjectiles + " projectiles, projectiles left " +  numberOfProjectilesInCharger);
		//rechargeNotify()

		return reloadedProjectiles;
	}

	public bool IsChargerFull()
	{
		return numberOfProjectilesInCharger == chargerCapacity;
	}
}
