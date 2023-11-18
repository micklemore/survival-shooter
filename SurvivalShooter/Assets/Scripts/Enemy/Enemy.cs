using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
	[SerializeField]
	FactionEnum faction = FactionEnum.ENEMY_FACTION;

	[SerializeField]
	float speed = 10f;

	[SerializeField]
	float health = 110f;

	EnemyFSM enemyFSM;

	bool isDead = false;

	bool canBePushedBack = true;

	Rigidbody2D rb;

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
			ApplyPushBackForce(damageObject.PushDirection, damageObject.PushForce, damageObject.TimerUntilNextShoot);
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
		canBePushedBack = true;
	}

	void ApplyPushBackForce(Vector3 pushDirection, float pushForce, float timerUntilNextPushback)
	{
		enemyFSM.ChangeState(enemyFSM.pushedBack);
		Debug.Log("pushdirection is " + pushDirection + " e force is " + pushForce);
		rb.AddForce(pushDirection.normalized * pushForce, ForceMode2D.Impulse);
		enemyFSM.ChangeState(enemyFSM.idleState);
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
		if (!(enemyFSM.GetCurrentState() == EnemyStates.PUSHED_BACK))
		{
			transform.position += (Vector3)direction * speed * Time.fixedDeltaTime;
			enemyFSM.ChangeState(enemyFSM.movingState);
		}
	}

	public void SetAITargetTransform(Transform targetTransform)
	{
		EnemyAI enemyAI = GetComponent<EnemyAI>();
		if (enemyAI != null)
		{
			enemyAI.SetTargetTransform(targetTransform);
		}
	}
}
