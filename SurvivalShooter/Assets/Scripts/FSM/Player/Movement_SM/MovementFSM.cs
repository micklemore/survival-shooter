using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementFSM: StateMachine
{
	[HideInInspector]
	public Idle idleState;

	[HideInInspector]
	public Moving movingState;

	void Awake()
	{
		this.idleState = new Idle(this);
		this.movingState = new Moving(this);
	}

	public override IBaseState GetInitialState()
	{
		return idleState;
	}

	public PlayerStates GetCurrentState()
	{
		if (CurrentState is Moving)
		{
			return PlayerStates.MOVING;
		} else
		{
			return PlayerStates.IDLE;
		}
	}

	private void OnGUI()
	{
		string content = CurrentState != null ? CurrentState.ToString() : "(no current state)";
		GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
	}
}
