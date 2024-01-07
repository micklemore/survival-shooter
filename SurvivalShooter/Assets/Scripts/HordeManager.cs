using SmartMVC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
	[SerializeField]
	Enemy enemy;

	[SerializeField]
	Player player;

	[SerializeField]
	float rightBound;

	[SerializeField]
	float upperBound;

	[SerializeField]
    int numberOfEnemiesToSpawn;

	[SerializeField]
	float minDistanceFromPlayer = 30f;

    float secondsUntilNextHordeStart = 3f;

	int currentEnemiesInPlay = 0;

	int hordeNumber = 0;

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, new Vector3(2 * rightBound, 2 * upperBound));
	}

	void Start()
    {
		StartCoroutine(WaitUntilNexitHordeStart(secondsUntilNextHordeStart));
	}

    IEnumerator WaitUntilNexitHordeStart(float timeUntilNextHordeStart)
    {
        yield return new WaitForSeconds(timeUntilNextHordeStart);
        StartHorde();
	}

    void StartHorde()
    {
		hordeNumber++;
		SpawnEnemies(numberOfEnemiesToSpawn);
		GameUIApplication.instance.Notify(MVCEvent.GameUIEvent.NEW_HORDE_STARTED, hordeNumber);
		GameUIApplication.instance.Notify(MVCEvent.GameUIEvent.ENEMY_COUNT_MODIFY, numberOfEnemiesToSpawn, numberOfEnemiesToSpawn);
    }

	public void SpawnEnemies(int numberOfEnemiesToSpawn)
	{
		for (int i = 0; i < numberOfEnemiesToSpawn; i++)
		{
			Vector2 enemySpawnPosition = GetEnemySpawnPosition();

			Debug.Log("Spawn point is " + enemySpawnPosition);
			
			SpawnEnemyAtPosition(enemySpawnPosition);
		}
	}

	void SpawnEnemyAtPosition(Vector2 enemySpawnPosition)
	{
		Enemy newEnemy = Instantiate(enemy, enemySpawnPosition, Quaternion.identity);
		newEnemy.SetAITargetTransform(player.transform);

		newEnemy.enemyDeathNotify += HandleEnemyDeath;
		currentEnemiesInPlay++;
	}

	Vector2 GetEnemySpawnPosition()
	{
		Vector2 enemySpawnPoint = GetRandomPosition();
		while (IsSpawnPointTooCloseToPlayer(enemySpawnPoint))
		{
			enemySpawnPoint = GetRandomPosition();
		}
		return enemySpawnPoint;
	}
	Vector2 GetRandomPosition()
	{
		float xPosition = Random.Range(-rightBound, +rightBound);
		float yPosition = Random.Range(-upperBound, +upperBound);


		return new Vector2(xPosition, yPosition) + (Vector2)transform.position;
	}

	bool IsSpawnPointTooCloseToPlayer(Vector2 position)
	{
		return Mathf.Abs((position - (Vector2)player.transform.position).sqrMagnitude) <= minDistanceFromPlayer;
	}

	public void HandleEnemyDeath(Enemy enemy)
	{
		enemy.enemyDeathNotify -= HandleEnemyDeath;
		
		currentEnemiesInPlay--;

		GameUIApplication.instance.Notify(MVCEvent.GameUIEvent.ENEMY_COUNT_MODIFY, currentEnemiesInPlay, numberOfEnemiesToSpawn);

		if (currentEnemiesInPlay == 0)
		{
			OnAllEnemiesDefeated();
		}
	}

	void OnAllEnemiesDefeated()
	{
		Debug.Log("Horde completed, waiting to start a new one");
		StartCoroutine(WaitUntilNexitHordeStart(secondsUntilNextHordeStart));
	}
}
