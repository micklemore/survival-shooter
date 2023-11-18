using UnityEngine;

public class NotAttacking : IBaseState
{
	AttackFSM attackFSM;

	public NotAttacking(AttackFSM attackFSM)
	{
		this.attackFSM = attackFSM;
	}

	public void OnEnter()
	{
		Debug.Log("OnEnter di NotAttacking");
	}

	public void OnExit()
	{
		Debug.Log("OnExit di NotAttacking");
	}

	public void UpdateLogic()
	{
	}

	public void UpdatePhysics()
	{
	}
}
