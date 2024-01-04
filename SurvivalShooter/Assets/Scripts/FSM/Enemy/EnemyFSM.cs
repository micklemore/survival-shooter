using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : StateMachine
{
	[HideInInspector]
	public EnemyIdle idleState;

	[HideInInspector]
	public EnemyMoving movingState;

	[HideInInspector]
	public EnemyHit hitState;

	[HideInInspector]
	public EnemyAttack attackState;

	Enemy enemy;

	void Awake()
	{
		this.idleState = new EnemyIdle(this);
		this.movingState = new EnemyMoving(this);
		this.hitState = new EnemyHit(this);
		this.attackState = new EnemyAttack(this);
	}

	public override BaseState GetInitialState()
	{
		return idleState;
	}

	public override void InitializeStateMachine()
	{
		enemy = GetComponent<Enemy>();
	}

	public EnemyStates GetCurrentState()
	{
		if (CurrentState is EnemyMoving) 
		{
			return EnemyStates.MOVING;
		} else if (CurrentState is EnemyHit)
		{
			return EnemyStates.HIT;
		} else if (CurrentState is EnemyIdle)
		{
			return EnemyStates.IDLE;
		} else
		{
			return EnemyStates.ATTACKING;
		}
	}

	public override void NotifyOnEnterState(string stateName)
	{
		enemy.RefreshAnimationForNewState(stateName);
	}
}
