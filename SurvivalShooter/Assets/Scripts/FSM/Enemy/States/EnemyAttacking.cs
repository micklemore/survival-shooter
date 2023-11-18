using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacking : IBaseState
{
	EnemyFSM enemyFSM;

	public EnemyAttacking(EnemyFSM enemyFSM)
	{
		this.enemyFSM = enemyFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Attack di Enemy");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Attack di Enemy");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
