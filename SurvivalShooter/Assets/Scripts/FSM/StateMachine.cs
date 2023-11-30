using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public BaseState CurrentState => currentState;
    BaseState currentState;
	
	void Start()
	{
		InitializeStateMachine();
		currentState = GetInitialState();
		Debug.Log("current state is " + currentState.ToString());
		if (currentState != null )
		{
			currentState.OnEnter();
		}
	}

	void Update()
	{
		if (currentState != null)
		{
			currentState.UpdateLogic();
		}
	}

	void FixedUpdate()
	{
		if (currentState != null)
		{
			currentState.UpdatePhysics();
		}
	}

	public void ChangeState(BaseState newState)
	{
		if (currentState == null)
		{
			currentState = newState;
			currentState.OnEnter();
		}
		else
		{
			if (currentState != newState)
			{
				currentState.OnExit();
				currentState = newState;
				currentState.OnEnter();
			}
		}
	}

	public virtual BaseState GetInitialState()
	{
		return null;
	}

	public virtual void InitializeStateMachine() { }

	public virtual void NotifyOnEnterState(string stateName) {}
}
