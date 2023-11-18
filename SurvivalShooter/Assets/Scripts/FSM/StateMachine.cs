using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	public IBaseState CurrentState => currentState;
    IBaseState currentState;

	void Start()
	{
		currentState = GetInitialState();
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

	public void ChangeState(IBaseState newState)
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

	public virtual IBaseState GetInitialState()
	{
		return null;
	}
}
