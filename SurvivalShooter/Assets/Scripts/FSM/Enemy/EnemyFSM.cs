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
	public EnemyHit hit;

	Enemy enemy;

	void Awake()
	{
		this.idleState = new EnemyIdle(this);
		this.movingState = new EnemyMoving(this);
		this.hit = new EnemyHit(this);
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
		if (CurrentState is PlayerMoving) 
		{
			return EnemyStates.MOVING;
		} else if (CurrentState is EnemyHit)
		{
			return EnemyStates.HIT;
		} else
		{
			return EnemyStates.IDLE;
		}
	}

	public override void NotifyOnEnterState(string stateName)
	{
		Debug.Log("notify enter state " + stateName);
		enemy.RefreshAnimationForNewState(stateName);
	}

	private void OnGUI()
	{
		string content = CurrentState != null ? CurrentState.ToString() : "(no current state)";
		GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
	}
}
