using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected StateMachine stateMachine;

    public BaseState(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

	public virtual void OnEnter()
    {
		stateMachine.NotifyOnEnterState(GetName());
	}

	public virtual void OnExit()
    {

    }

	public virtual void UpdateLogic()
    {

    }

	public virtual void UpdatePhysics()
    {

    }

    protected abstract string GetName();
}
