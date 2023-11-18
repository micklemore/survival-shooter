using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : StateMachine
{
	[HideInInspector]
	public EnemyIdle idleState;

	[HideInInspector]
	public EnemyAttacking attackingState;

	[HideInInspector]
	public EnemyMoving movingState;

	[HideInInspector]
	public EnemyPushedBack pushedBack;

	void Awake()
	{
		this.idleState = new EnemyIdle(this);
		this.attackingState = new EnemyAttacking(this);
		this.movingState = new EnemyMoving(this);
		this.pushedBack = new EnemyPushedBack(this);
	}

	public override IBaseState GetInitialState()
	{
		return idleState;
	}

	public EnemyStates GetCurrentState()
	{
		if (CurrentState is EnemyAttacking)
		{
			return EnemyStates.ATTACKING;
		} else if (CurrentState is Moving) 
		{
			return EnemyStates.MOVING;
		} else if (CurrentState is EnemyPushedBack)
		{
			return EnemyStates.PUSHED_BACK;
		} else
		{
			return EnemyStates.IDLE;
		}
	}
}
