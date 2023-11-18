using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving : IBaseState
{
	MovementFSM movementFSM;

	public Moving(MovementFSM movementFSM)
	{
		this.movementFSM = movementFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Moving");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Moving");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
