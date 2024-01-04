using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyAttack : BaseState
{
	static string NAME = "attack";
	
	public EnemyAttack(StateMachine stateMachine) : base(stateMachine) {}

	public override void OnEnter(params object[] data)
	{
		base.OnEnter(data);

		FactionEnum faction = (FactionEnum)data[0];
		Vector3 enemyPosition = (Vector3)data[1];
		Vector3 playerPosition = (Vector3)data[2];
		float circleColliderDistance = (float)data[3];
		float circleColliderRadius = (float)data[4];
		float damage = (float)data[5];
		float pushbackForce = (float)data[6];

		Vector2 circleColliderPosition = GetCircleColliderPosition(enemyPosition, playerPosition, circleColliderDistance);
		Collider2D[] collisions = Physics2D.OverlapCircleAll(circleColliderPosition, circleColliderRadius);

		foreach (Collider2D collision in collisions)
		{
			IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
			if (damageable != null && faction != damageable.GetFaction())
			{
				ContactPoint2D[] contactPoints = new ContactPoint2D[1];
				collision.GetContacts(contactPoints);

				Vector3 pushDirection = (Vector3)contactPoints[0].point - (Vector3)circleColliderPosition;
				ApplyDamageToDamageables(damageable, pushDirection, damage, pushbackForce);
			}
		}
		//stateMachine.ChangeState(((EnemyFSM)stateMachine).idleState);
	}

	public void ApplyDamageToDamageables(IDamageable damageable, Vector3 pushDirection, float damage, float pushbackForce)
	{
		DamageObject damageObject = new DamageObject(damage, pushDirection, pushbackForce, 0);
		HitResult hitResult = damageable.TakeDamage(damageObject);

	}

	Vector2 GetCircleColliderPosition(Vector3 enemyPosition, Vector3 playerPosition, float circleColliderDistance)
	{
		Vector2 directionToPlayer = (Vector2)(playerPosition - enemyPosition).normalized;
		Vector2 circleColliderPosition = (Vector2)enemyPosition + directionToPlayer * circleColliderDistance;
		Debug.DrawRay(circleColliderPosition, directionToPlayer);
		return circleColliderPosition;
	}

	protected override string GetName()
	{
		return NAME;
	}
}
