using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private bool playerInside = false; // Is the player in range to hide?
    private bool isHiding = false; // Is the player currently hiding?
    private GameObject player;
    private SpriteRenderer playerSprite;
    private Camera mainCamera;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Camera offset
    [SerializeField] private KeyCode hideKey = KeyCode.E; // Key to hide/unhide
    [SerializeField] private bool useTrigger = true; // Toggle between collision or trigger

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(hideKey))
        {
            if (!isHiding)
            {
                EnterHiding();
            }
            else
            {
                ExitHiding();
            }
        }
    }

    private void EnterHiding()
    {
        if (player == null) return;

        isHiding = true;
        playerSprite.enabled = false; // Hide player sprite
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z); // Center camera
    }

    private void ExitHiding()
    {
        isHiding = false;
        playerSprite.enabled = true; // Show player sprite
        mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z); // Reset camera
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
            player = collision.gameObject;
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!useTrigger && collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;

            if (isHiding)
            {
                ExitHiding();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (useTrigger && collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
            player = collision.gameObject;
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (useTrigger && collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;

            if (isHiding)
            {
                ExitHiding();
            }
        }
    }
}
