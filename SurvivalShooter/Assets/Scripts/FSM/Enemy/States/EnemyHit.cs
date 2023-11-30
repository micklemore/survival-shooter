using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : BaseState
{
	static string NAME = "hit";

	public EnemyHit(StateMachine stateMachine) : base(stateMachine) {}

	protected override string GetName()
	{
		return NAME;
	}
}
