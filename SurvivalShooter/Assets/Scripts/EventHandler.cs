using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler instance;

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(this);
		}
		instance = this;
	}


	public delegate void EndGame();
	public EndGame endGameDelegate;

	public void EndGameNotify()
	{
		endGameDelegate?.Invoke();
	}
}
