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
	float attackDamage;

	[SerializeField]
	float pushbackForce;

	[SerializeField]
	float attackDistance = 3f;
	public float AttackDistance => attackDistance;

    [SerializeField]
	float pushbackMultiplier = 0.1f;

	[SerializeField]
	float circleColliderDistance = 0.2f;

	[SerializeField]
	float circleColliderRadius = 0.4f;

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

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(new Vector2(transform.position.x , transform.position.y) * circleColliderDistance, circleColliderRadius);
	}

	public FactionEnum GetFaction() => faction;

	public HitResult TakeDamage(DamageObject damageObject)
	{
		HitResult hitResult = new HitResult();
		hitResult.SetDamage(damageObject.DamageAmount);

		if (canBePushedBack)
		{
			canBePushedBack = false;
			enemyFSM.ChangeState(enemyFSM.hitState);
			ApplyPushBackForceToSelf(damageObject.PushDirection, damageObject.PushForce, damageObject.TimerUntilNextShoot);
		}

		health -= damageObject.DamageAmount;

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
		rb.AddForce(pushDirection.normalized * pushForce * pushbackMultiplier, ForceMode2D.Impulse);
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
			enemyFSM.ChangeState(enemyFSM.idleState);
		}
	}

	public void Attack(Vector3 playerPosition)
	{
		enemyFSM.ChangeState(enemyFSM.attackState, faction, transform.position, playerPosition, circleColliderDistance, circleColliderRadius, attackDamage, pushbackForce);
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
