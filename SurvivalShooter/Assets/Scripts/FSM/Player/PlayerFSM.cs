using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM: StateMachine
{
	[HideInInspector]
	public PlayerIdle idleState;

	[HideInInspector]
	public PlayerMoving movingState;

	Player player;

	void Awake()
	{
		this.idleState = new PlayerIdle(this);
		this.movingState = new PlayerMoving(this);
	}

	public override BaseState GetInitialState()
	{
		return idleState;
	}

	public override void InitializeStateMachine()
	{
		player = GetComponent<Player>();
	}

	public PlayerStates GetCurrentState()
	{
		if (CurrentState is PlayerMoving)
		{
			return PlayerStates.MOVING;
		} else
		{
			return PlayerStates.IDLE;
		}
	}

	public override void NotifyOnEnterState(string stateName)
	{
		player.RefreshAnimationForNewState(stateName);
	}
}
