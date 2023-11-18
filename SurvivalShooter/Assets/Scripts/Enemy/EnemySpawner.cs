using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField]
	Enemy enemy;

	[SerializeField]
	Player player;

	List<Enemy> enemiesSpawned = new List<Enemy>();

	public void SpawnEnemies(int numberOfEnemiesToSpawn)
	{
		for (int i = 0; i < numberOfEnemiesToSpawn; i++)
		{
			Enemy newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
			newEnemy.SetAITargetTransform(player.transform);

			newEnemy.enemyDeathNotify += HandleEnemyDeath;
			enemiesSpawned.Add(newEnemy);
		}
	}

	public void HandleEnemyDeath(Enemy enemy)
	{
		enemy.enemyDeathNotify -= HandleEnemyDeath;
		enemiesSpawned.Remove(enemy);
		
		if (enemiesSpawned.Count == 0)
		{
			OnAllEnemiesDefeated();
		}
	}

	public delegate void AllEnemeiesDefeated();
	public AllEnemeiesDefeated enemiesDefeatedNotify;

	void OnAllEnemiesDefeated()
	{
		Debug.Log("I'm the spawner " + gameObject.name + " and all the enemies I created has been defeated");
		enemiesDefeatedNotify?.Invoke();
	}
}
