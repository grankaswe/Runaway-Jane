using UnityEngine;

public class EnemyStealthAI : MonoBehaviour
{
    public Transform player;
    public float moveSpeed = 2f;
    public LayerMask obstacleLayer;
    public float chaseTime = 3f;
    public Collider2D visionCollider; // Field of vision
    public Collider2D soundCollider;  // Field of sound detection
    public EnemyMovementAI movementAI; // Reference to movement script

    private bool isChasing = false;
    private float lostPlayerTime;
    private bool playerInVision = false;
    private bool playerInSoundRange = false;
    private HidingSpot playerHidingSpot; // Reference to hiding spot

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
        CheckPlayerInSoundRange();

        if (isChasing)
        {
            if ((playerInVision && CanSeePlayer()) || playerInSoundRange)
            {
                ChasePlayer();
                lostPlayerTime = Time.time;
            }
            else if (Time.time - lostPlayerTime > chaseTime)
            {
                isChasing = false;
                if (movementAI != null)
                {
                    movementAI.StartPatrolling(); // Resume patrolling
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (!(playerInVision && CanSeePlayer()) && !playerInSoundRange) return;

        if (movementAI != null)
        {
            movementAI.StartChasing(player);
        }
    }

    bool CanSeePlayer()
    {
        if (playerHidingSpot != null && playerHidingSpot.IsHiding) return false; // Player is hiding, AI cannot see them

        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer);

        return hit.collider == null || hit.collider.gameObject == player.gameObject;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHidingSpot = other.GetComponent<HidingSpot>(); // Get hiding spot component from player

            if (playerHidingSpot == null || !playerHidingSpot.IsHiding)
            {
                if (other == visionCollider) playerInVision = true;
                if (other == soundCollider) playerInSoundRange = true;
                isChasing = true;
                lostPlayerTime = Time.time;
                if (movementAI != null) movementAI.StartChasing(player);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other == visionCollider) playerInVision = false;
            if (other == soundCollider) playerInSoundRange = false;
        }
    }

    void CheckPlayerInVision()
    {
        if (visionCollider != null && player != null)
        {
            playerInVision = visionCollider.bounds.Contains(player.position) && CanSeePlayer();
        }
    }

    void CheckPlayerInSoundRange()
    {
        if (soundCollider != null && player != null)
        {
            if (playerHidingSpot != null && playerHidingSpot.IsHiding) return; // Ignore if hiding

            float playerNoiseLevel = GetPlayerNoiseLevel();
            float detectionRadius = soundCollider.bounds.extents.magnitude * playerNoiseLevel;
            playerInSoundRange = Vector2.Distance(transform.position, player.position) <= detectionRadius;
        }
    }

    float GetPlayerNoiseLevel()
    {
        if (player == null) return 0f;
        TopDownMovement playerMovement = player.GetComponent<TopDownMovement>();
        if (playerMovement == null) return 0f;

        if (Input.GetKey(KeyCode.LeftShift)) return 1.5f;
        if (Input.GetKey(KeyCode.LeftControl)) return 0.5f;
        return 1f;
    }
}
