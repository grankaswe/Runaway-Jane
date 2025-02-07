using UnityEngine.AI;
using UnityEngine;
using System.Collections.Generic;

public class EnemyMovementAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;
    private bool isChasing = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
        if (isChasing && target != null)
        {
            agent.SetDestination(target.position);
            RotateTowards((Vector2)agent.steeringTarget);
        }
    }

    public void StartChasing(Transform player)
    {
        isChasing = true;
        target = player;
    }

    public void StartPatrolling()
    {
        isChasing = false;
        StartCoroutine(PatrolRoutine());
    }

    private System.Collections.IEnumerator PatrolRoutine()
    {
        while (!isChasing)
        {
            Vector2 randomPoint = GetRandomPoint();
            agent.SetDestination(randomPoint);
            RotateTowards(randomPoint);

            yield return new WaitForSeconds(Random.Range(3f, 7f)); // Random wait time before moving again
        }
    }

    private Vector2 GetRandomPoint()
    {
        Vector2 randomDirection = Random.insideUnitCircle * 5f;
        randomDirection += (Vector2)transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return (Vector2)transform.position;
    }

    private void RotateTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
