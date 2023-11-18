using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : IBaseState
{
	EnemyFSM enemyFSM;

	public EnemyMoving(EnemyFSM enemyFSM)
	{
		this.enemyFSM = enemyFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Moving di Enemy");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Moving di Enemy");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
