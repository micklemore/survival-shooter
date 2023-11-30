using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable, IDamager
{
	[SerializeField]
	FactionEnum faction = FactionEnum.ENEMY_FACTION;

	[SerializeField]
	float speed = 10f;

	[SerializeField]
	float health = 110f;

	[SerializeField]
	Animator animator;

	EnemyFSM enemyFSM;

	bool isDead = false;

	bool canBePushedBack = true;

	Rigidbody2D rb;

	string currentStateName;

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		enemyFSM = GetComponent<EnemyFSM>();
	}

	public FactionEnum GetFaction() => faction;

	public HitResult TakeDamage(DamageObject damageObject)
	{
		HitResult hitResult = new HitResult();
		hitResult.SetDamage(damageObject.DamageAmount);

		if (canBePushedBack)
		{
			canBePushedBack = false;
			enemyFSM.ChangeState(enemyFSM.hit);
			ApplyPushBackForceToSelf(damageObject.PushDirection, damageObject.PushForce, damageObject.TimerUntilNextShoot);
		}

		health -= damageObject.DamageAmount;
		Debug.Log("Health = " + health);

		if (!isDead && health <= 0)
		{
			isDead = true;
			HandleDeath();
		}
		hitResult.SetDead(isDead);
		return hitResult;
	}

	IEnumerator WaitForNextPushback(float timerUntilNextPushback)
	{
		yield return new WaitForSeconds(timerUntilNextPushback);
		enemyFSM.ChangeState(enemyFSM.idleState);
		canBePushedBack = true;
	}

	void ApplyPushBackForceToSelf(Vector3 pushDirection, float pushForce, float timerUntilNextPushback)
	{
		Debug.Log("pushdirection is " + pushDirection + " e force is " + pushForce);
		rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
		StartCoroutine(WaitForNextPushback(timerUntilNextPushback));
	}

	public delegate void EnemyDeath(Enemy enemy);
	public EnemyDeath enemyDeathNotify;

	void HandleDeath()
	{
		enemyDeathNotify?.Invoke(this);
		Destroy(gameObject);
	}

	public void Move(Vector2 direction)
	{
		if (!(enemyFSM.GetCurrentState() == EnemyStates.HIT))
		{
			transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
			enemyFSM.ChangeState(enemyFSM.movingState);
		}
	}

	public void Attack(Transform playerTransform)
	{
		Vector2 circleColliderPosition = GetCircleColliderPosition(playerTransform);
		Collider2D[] collisions = Physics2D.OverlapCircleAll(circleColliderPosition, 0.5f);

		foreach (Collider2D collision in collisions)
		{
			IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
			if (damageable != null && faction != damageable.GetFaction())
			{
				Debug.Log("ho colpito un " + collision.gameObject.name);
				ContactPoint2D[] contactPoints = new ContactPoint2D[1];
				collision.GetContacts(contactPoints);

				Vector3 pushDirection = (Vector3)contactPoints[0].point - (Vector3)circleColliderPosition;
				ApplyDamageToDamageables(damageable, pushDirection);
			}
		}
	}

	public void ApplyDamageToDamageables(IDamageable damageable, Vector3 pushDirection)
	{
		//DamageObject damageObject = new DamageObject(damage, pushDirection, base.pushbackForce, base.timerUntilNextPushback);
		//HitResult hitResult = damageable.TakeDamage(damageObject);

		//Debug.Log("ho fatto " + hitResult.Damage + "danni");
	}

	Vector2 GetCircleColliderPosition(Transform playerTransform)
	{
		Vector2 directionToPlayer = (Vector2)(playerTransform.position - transform.position).normalized;
		Vector2 circleColliderPosition = (Vector2)transform.position + directionToPlayer * 3;
		return circleColliderPosition;
	}

	public void SetAITargetTransform(Transform targetTransform)
	{
		EnemyAI enemyAI = GetComponent<EnemyAI>();
		if (enemyAI != null)
		{
			enemyAI.SetTargetTransform(targetTransform);
		}
	}

	public void RefreshAnimationForNewState(string stateName)
	{
		currentStateName = stateName;
		PlayAnimation(stateName);
	}

	public void PlayAnimation(string animationName)
	{
		animator.Play(animationName);
	}
}
