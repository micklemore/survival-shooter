using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitResult
{
	public float Damage => damage;
	private float damage;

	public float HealthLeft => healthLeft;
	private float healthLeft;

	public bool IsDead => isDead;
	private bool isDead;

	public void SetDamage(float damage)
	{
		this.damage = damage;
	}

	public void SetHealthLeft(float healthLeft)
	{
		this.healthLeft = healthLeft;
	}

	public void SetDead(bool isDead)
	{
		this.isDead = isDead;
	}
}