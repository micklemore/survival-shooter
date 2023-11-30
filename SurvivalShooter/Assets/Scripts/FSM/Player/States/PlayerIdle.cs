using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerIdle : BaseState
{
	static string NAME = "idle";

	public PlayerIdle(StateMachine stateMachine) : base(stateMachine) {}

	protected override string GetName()
	{
		return NAME;
	}
}
