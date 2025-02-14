using UnityEngine;

public class EnemyStealthAI : MonoBehaviour
{
    public float visionRange = 5f;
    public float hearingRange = 3f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private HidingSpot hidingSpot;
    private bool playerDetected = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        hidingSpot = FindObjectOfType<HidingSpot>();
    }

    void Update()
    {
        if (hidingSpot != null && hidingSpot.IsHiding)
        {
            playerDetected = false;
            return; // Skip detection logic if player is hiding
        }

        DetectPlayer();
    }

    void DetectPlayer()
    {
        if (Vector2.Distance(transform.position, player.position) <= visionRange)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.position, obstacleLayer);
            if (!hit.collider)
            {
                playerDetected = true;
                return;
            }
        }

        if (Vector2.Distance(transform.position, player.position) <= hearingRange)
        {
            playerDetected = true;
        }
        else
        {
            playerDetected = false;
        }
    }
}
