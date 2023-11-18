using UnityEngine;

public class EnemyIdle : IBaseState
{
	EnemyFSM enemyFSM;

	public EnemyIdle(EnemyFSM enemyFSM)
	{
		this.enemyFSM = enemyFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di Idle di Enemy");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di Idle di Enemy");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}

	public override string ToString()
	{
		return "Idle";
	}
}
