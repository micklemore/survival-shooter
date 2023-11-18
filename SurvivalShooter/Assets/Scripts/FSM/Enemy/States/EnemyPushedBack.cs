using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPushedBack : IBaseState
{
	EnemyFSM enemyFSM;

	public EnemyPushedBack(EnemyFSM enemyFSM)
	{
		this.enemyFSM = enemyFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di PushedBack di Enemy");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di PushedBack di Enemy");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
