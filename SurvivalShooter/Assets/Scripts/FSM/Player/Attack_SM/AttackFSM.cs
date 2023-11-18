using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackFSM : StateMachine
{
	[HideInInspector]
	public Attacking attackingState;

	[HideInInspector]
	public NotAttacking notAttackingState;

	void Awake()
	{
		this.attackingState = new Attacking(this);
		this.notAttackingState = new NotAttacking(this);
	}

	public override IBaseState GetInitialState()
	{
		return notAttackingState;
	}

	public PlayerStates GetCurrentState()
	{
		if (CurrentState is Attacking)
		{
			return PlayerStates.ATTACKING;
		} else
		{
			return PlayerStates.NOT_ATTACKING;
		}
	}
}
