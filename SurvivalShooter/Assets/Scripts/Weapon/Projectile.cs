using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamager
{
	protected float speed;

	protected float damage;

	protected float range;

	protected FactionEnum faction;

	protected float pushbackForce;

	protected float timerUntilNextPushback;

	Weapon owner;

	Vector3 startPosition;

	Vector3 projectileDirection;

	void FixedUpdate()
	{
		transform.position += projectileDirection.normalized * speed * Time.fixedDeltaTime;
		
		CheckIfOProjectileHasReachedMaxRange();
	}

	public void FireProjectile(Weapon owner, Vector3 directionToFollow, float damage, float speed, float range, FactionEnum faction, float pushbackForce, float timerUntilNextPushback)
	{
		IntializeProjectile(directionToFollow, damage, speed, range, faction, pushbackForce, timerUntilNextPushback);
	}

	void IntializeProjectile(Vector3 directionToFollow, float damage, float speed, float range, FactionEnum faction, float pushbackForce, float timerUntilNextPushback)
	{
		startPosition = transform.position;

		directionToFollow.z = 0f;
		projectileDirection = directionToFollow;

		this.damage = damage;
		this.range = range;
		this.speed = speed;
		this.faction = faction;
		this.pushbackForce = pushbackForce;
		this.timerUntilNextPushback = timerUntilNextPushback;
	}

	void CheckIfOProjectileHasReachedMaxRange()
	{
		if ((transform.position - startPosition).sqrMagnitude >= range * range)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("ho fatto trigger con " + collision.name);
		IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
		if (damageable == null)
		{
			Destroy(gameObject);
		}
		else if (faction != damageable.GetFaction())
		{
			ApplyDamageToDamageables(damageable);
			Destroy(gameObject);
		}
	}

	public void ApplyDamageToDamageables(IDamageable damageable)
	{
		DamageObject damageObject = new DamageObject(damage, projectileDirection, pushbackForce, timerUntilNextPushback);
		HitResult hitResult = damageable.TakeDamage(damageObject);
	}
}
