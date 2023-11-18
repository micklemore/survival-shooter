using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : IBaseState
{
	AttackFSM attackFSM;

	public Attacking(AttackFSM attackFSM)
	{
		this.attackFSM = attackFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Attacking");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Attacking");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
