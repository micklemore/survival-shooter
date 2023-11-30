using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : BaseState
{
	static string NAME = "chasing";

	public EnemyMoving(StateMachine stateMachine) : base(stateMachine) {}

	protected override string GetName()
	{
		return NAME;
	}
}
