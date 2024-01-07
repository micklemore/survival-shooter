using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    float nextWaypointDistance = 3f;

    [SerializeField]
    float timerForNextPathCalculation = 0.1f;

    float currentTimer = 0;

    //bool reachedEndOfPath = false;

    int currentWaypoint = 0;

    Path currentPath;

    Enemy enemy;

    Seeker seeker;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemy = GetComponent<Enemy>();

        ComputePath();
	}

    void OnPathComputed(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.Log("There is an error on computed path: " + p.error);
        }
    }

    void ComputePath()
    {
        if (targetTransform.position != null)
        {
            seeker.StartPath(transform.position, targetTransform.position, OnPathComputed);
        }
	}

    void FixedUpdate()
    {
        if ((targetTransform.position - transform.position).sqrMagnitude <= enemy.AttackDistance)
        {
            enemy.Attack(targetTransform.position);
        }
        currentTimer += Time.fixedDeltaTime;
        if (currentTimer >= timerForNextPathCalculation)
        {
            currentTimer = 0;
            ComputePath();
        }

        if (currentPath == null)
        {
            return;
        }

        if (HasReachedEndOfPath()) { return; }

        MoveEnemy();

		float distanceToNextWaypoint = (currentPath.vectorPath[currentWaypoint] - transform.position).sqrMagnitude;
        if (distanceToNextWaypoint < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

	bool HasReachedEndOfPath()
    {
        if (currentWaypoint >= currentPath.path.Count)
        {
            return true;
        }
        return false;
    }

	void MoveEnemy()
    {
		Vector2 movementDirection = ((Vector2)currentPath.vectorPath[currentWaypoint] - (Vector2)transform.position).normalized;
		enemy.Move(movementDirection);
	}

    public void SetTargetTransform(Transform playerTransform)
    {
        targetTransform = playerTransform;
    }
}
