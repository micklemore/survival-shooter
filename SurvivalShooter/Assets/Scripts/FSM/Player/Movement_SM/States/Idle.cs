using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Idle : IBaseState
{
	MovementFSM movementFSM;

	public Idle(MovementFSM movementFSM)
	{
		this.movementFSM = movementFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Idle");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Idle");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
