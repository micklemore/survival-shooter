using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
	HitResult TakeDamage(DamageObject damageObject);

	FactionEnum GetFaction();
}
