using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyStealthAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    public float chaseTime = 3f;
    public Transform[] waypoints;
    public Collider2D visionCollider; // Field of vision

    private bool isChasing = false;
    private float lostPlayerTime;
    private Transform currentWaypoint;
    private Dictionary<Transform, List<Transform>> waypointGraph;
    private bool playerInVision = false; // Track if player is inside vision collider

    void Start()
    {
        BuildWaypointGraph();
        currentWaypoint = waypoints[0];
        transform.position = currentWaypoint.position;
    }

    void Update()
    {
        CheckPlayerInVision();

        if (isChasing)
        {
            if (playerInVision && CanSeePlayer())
            {
                ChasePlayer();
                lostPlayerTime = Time.time;
            }
            else if (Time.time - lostPlayerTime > chaseTime)
            {
                isChasing = false;
                ReturnToNearestWaypoint();
            }
        }
        else
        {
            PatrolBetweenWaypoints();
        }
    }

    void BuildWaypointGraph()
    {
        waypointGraph = new Dictionary<Transform, List<Transform>>();
        for (int i = 0; i < waypoints.Length; i++)
        {
            List<Transform> neighbors = new List<Transform>();
            if (i > 0) neighbors.Add(waypoints[i - 1]);
            if (i < waypoints.Length - 1) neighbors.Add(waypoints[i + 1]);
            if (i > 1) neighbors.Add(waypoints[i - 2]);
            if (i < waypoints.Length - 2) neighbors.Add(waypoints[i + 2]);
            waypointGraph[waypoints[i]] = neighbors;
        }
    }

    void PatrolBetweenWaypoints()
    {
        if (currentWaypoint == null || waypointGraph.Count == 0) return;
        if (Vector2.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            List<Transform> possibleNextWaypoints = waypointGraph[currentWaypoint];
            float forwardChance = 0.75f; // Increase chance of moving forward
            if (possibleNextWaypoints.Count > 1 && Random.value > forwardChance)
            {
                currentWaypoint = possibleNextWaypoints[Random.Range(1, possibleNextWaypoints.Count)];
            }
            else
            {
                currentWaypoint = possibleNextWaypoints[0];
            }
        }
        MoveTowards(currentWaypoint.position);
    }

    void ChasePlayer()
    {
        if (!playerInVision || !CanSeePlayer()) return; // Stop movement if the player isn't seen

        Vector2 direction = (player.position - transform.position).normalized;
        if (!BlockedByWall(direction))
        {
            MoveTowards(player.position);
        }
    }

    void ReturnToNearestWaypoint()
    {
        Transform nearest = waypoints.OrderBy(w => Vector2.Distance(transform.position, w.position)).First();
        currentWaypoint = nearest;
        MoveTowards(currentWaypoint.position);
    }

    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);
        return hit.collider == null;
    }

    bool BlockedByWall(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, direction, 1f, obstacleLayer);
        return hit.collider != null;
    }

    void MoveTowards(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = true;
            isChasing = true;
            lostPlayerTime = Time.time;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = false;
        }
    }
    
    void CheckPlayerInVision()
    {
        if (visionCollider != null && player != null)
        {
            playerInVision = visionCollider.bounds.Contains(player.position);
        }
    }
}
