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

    private bool isChasing = false;
    private float lostPlayerTime;
    private Transform currentWaypoint;
    private Dictionary<Transform, List<Transform>> waypointGraph;
    private Transform previousWaypoint;

    void Start()
    {
        BuildWaypointGraph();
        currentWaypoint = waypoints[0];
        transform.position = currentWaypoint.position;
    }

    void Update()
    {
        if (isChasing)
        {
            if (CanSeePlayer())
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
            possibleNextWaypoints = possibleNextWaypoints.OrderBy(wp => wp == previousWaypoint ? 1 : 0).ToList(); // Lower chance to go back
            previousWaypoint = currentWaypoint;
            currentWaypoint = possibleNextWaypoints[Random.Range(0, possibleNextWaypoints.Count)];
        }

        transform.position = Vector2.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
    }

    void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        if (!BlockedByWall(direction))
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
    }

    void ReturnToNearestWaypoint()
    {
        Transform nearest = waypoints.OrderBy(w => Vector2.Distance(transform.position, w.position)).First();
        currentWaypoint = nearest;
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
}
