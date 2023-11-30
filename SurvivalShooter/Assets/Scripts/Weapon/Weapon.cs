using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
	[SerializeField]
	Transform projectileSpawnSocketTransform;

	[SerializeField]
	Projectile projectile;

	[SerializeField]
	IDamager damager;

	[SerializeField]
	GameObject burstEffect;

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
			StartCoroutine(SpawnBurstEffect());
			Projectile projectileToSpawn = Instantiate(projectile, projectileSpawnSocketTransform.position, transform.rotation);
			projectileToSpawn.FireProjectile(this, GetDirectionOfFire(), damage, projectileSpeed, rangeDistance, faction, pushbackForce, timerUntilNextPushback);
		}
		numberOfProjectilesInCharger--;
		//fireNotify()
		Debug.Log("projectiles left: " + numberOfProjectilesInCharger);
	}

	IEnumerator SpawnBurstEffect()
	{
		if (burstEffect)
		{
			GameObject burst = Instantiate(burstEffect, projectileSpawnSocketTransform);
			yield return null;
		}
	}

	bool CanShoot()
	{
		return timerUntilNextShoot <= 0 && (HasInfiniteCharger || numberOfProjectilesInCharger > 0);
	}

	protected Vector3 GetDirectionOfFire()
	{
		Vector3 directionOfFire = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
		Vector3 spreadDirection = new Vector3(directionOfFire.x *10 + Random.Range(-spread, spread), directionOfFire.y *10 + Random.Range(-spread, spread), 0);
		return spreadDirection.normalized;
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
