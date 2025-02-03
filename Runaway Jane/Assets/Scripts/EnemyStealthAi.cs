using UnityEngine;

public class EnemyStealthAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public Transform[] waypoints;              // Array of waypoints for patrolling
    public float chaseTime = 3f;               // Time before enemy gives up chasing player
    public float detectionRadius = 0.5f;       // Radius for detecting obstacles around the enemy

    private int currentWaypointIndex = 0;      // Index of the current waypoint
    private bool isChasing = false;            // Whether the enemy is chasing the player
    private float lostPlayerTime;              // Time when the player was lost
    private bool isReturningToWaypoint = false; // Whether the enemy is returning to a waypoint

    void Start()
    {
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position; // Start at the first waypoint
        }
    }

    void Update()
    {
        if (isChasing)
        {
            if (CanSeePlayer()) // If the enemy can see the player
            {
                ChasePlayer(); // Chase the player
                lostPlayerTime = Time.time; // Update last seen time
            }
            else if (Time.time - lostPlayerTime > chaseTime) // Time to stop chasing
            {
                isChasing = false; // Stop chasing after a delay
                ReturnToWaypoint();
            }
        }
        else
        {
            PatrolBetweenWaypoints(); // Continue patrolling if not chasing
        }
    }

    void PatrolBetweenWaypoints()
    {
        if (waypoints.Length == 0) return;

        // Move towards the current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);

        // Check if the enemy reached the current waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Move to the next waypoint in the array
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void ChasePlayer()
    {
        // Calculate the direction towards the player (2D vector)
        Vector2 direction = (player.position - transform.position).normalized;

        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void ReturnToWaypoint()
    {
        // Find the nearest waypoint to return to
        Transform nearestWaypoint = FindNearestWaypoint();

        // Move towards the nearest waypoint
        transform.position = Vector2.MoveTowards(transform.position, nearestWaypoint.position, moveSpeed * Time.deltaTime);

        // Check if the enemy has reached the nearest waypoint
        if (Vector2.Distance(transform.position, nearestWaypoint.position) < 0.1f)
        {
            isReturningToWaypoint = false;
            PatrolBetweenWaypoints(); // Start patrolling again
        }
    }

    Transform FindNearestWaypoint()
    {
        Transform nearestWaypoint = waypoints[0];
        float closestDistance = Vector2.Distance(transform.position, nearestWaypoint.position);

        // Loop through all waypoints to find the nearest one
        foreach (Transform waypoint in waypoints)
        {
            float distance = Vector2.Distance(transform.position, waypoint.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestWaypoint = waypoint;
            }
        }

        return nearestWaypoint;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanSeePlayer()) // Detect player within the trigger
        {
            isChasing = true;
            lostPlayerTime = Time.time; // Record the time when the player was detected
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // If the player exits the trigger area
        {
            lostPlayerTime = Time.time; // Record the time when the player was lost
        }
    }

    bool CanSeePlayer()
    {
        // Check if the enemy can see the player (just a simple check)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer < detectionRadius; // If the player is within the detection radius
    }
}
