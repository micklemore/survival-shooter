using UnityEngine;

public class EnemyIdle : BaseState
{
	static string NAME = "idle";

	public EnemyIdle(StateMachine stateMachine) : base(stateMachine) {}

	protected override string GetName()
	{
		return NAME;
	}
}
