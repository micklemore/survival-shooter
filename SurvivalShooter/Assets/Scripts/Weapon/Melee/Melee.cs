using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : Weapon, IDamager
{
	[SerializeField]
	Transform circleColliderTransform;

	[SerializeField]
	float circleColliderRadius;

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(new Vector2(circleColliderTransform.position.x, circleColliderTransform.position.y), circleColliderRadius);
	}

	public void ApplyDamageToDamageables(IDamageable damageable, Vector3 pushDirection)
	{
		DamageObject damageObject = new DamageObject(damage, pushDirection, base.pushbackForce, base.timerUntilNextPushback);
		HitResult hitResult = damageable.TakeDamage(damageObject);
	}

	public override void SpawnProjectilesOrMelee(FactionEnum faction)
	{
		Collider2D[] collisions = Physics2D.OverlapCircleAll(new Vector2(circleColliderTransform.position.x, circleColliderTransform.position.y), circleColliderRadius);

		foreach (Collider2D collision in collisions)
		{
			IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
			if (damageable != null && faction != damageable.GetFaction())
			{
				Debug.Log("ho colpito un " + collision.gameObject.name);
				ContactPoint2D[] contactPoints = new ContactPoint2D[1];
				collision.GetContacts(contactPoints);

				Vector3 pushDirection = (Vector3)contactPoints[0].point - circleColliderTransform.position;
				ApplyDamageToDamageables(damageable, pushDirection);
			}
		}
	}
}
