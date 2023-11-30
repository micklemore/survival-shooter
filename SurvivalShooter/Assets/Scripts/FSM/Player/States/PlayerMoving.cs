using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : BaseState
{
	static string NAME = "walk";

	public PlayerMoving(StateMachine stateMachine) : base(stateMachine) {}

	protected override string GetName()
	{
		return NAME;
	}
}
