using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyStealthAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    public float chaseTime = 3f;
    public Collider2D visionCollider; // Field of vision
    public EnemyMovementAI movementAI; // Reference to movement script

    private bool isChasing = false;
    private float lostPlayerTime;
    private bool playerInVision = false; // Track if player is inside vision collider

    void Start()
    {
        if (movementAI != null)
        {
            movementAI.enabled = true; // Enable movement AI initially for patrolling
        }
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
                if (movementAI != null)
                {
                    movementAI.StartPatrolling(); // Resume patrolling when the chase ends
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (!playerInVision || !CanSeePlayer()) return; // Stop movement if the player isn't seen

        if (movementAI != null)
        {
            movementAI.StartChasing(player); // Enable movement AI and chase player
        }
    }

    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);
        return hit.collider == null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInVision = true;
            isChasing = true;
            lostPlayerTime = Time.time;
            if (movementAI != null) movementAI.StartChasing(player);
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
