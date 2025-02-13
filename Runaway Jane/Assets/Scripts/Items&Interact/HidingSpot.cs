using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private bool playerInside = false; // Is the player in range to hide?
    private bool isHiding = false; // Is the player currently hiding?
    private GameObject player;
    private SpriteRenderer playerSprite;
    private Camera mainCamera;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Camera position offset
    [SerializeField] private KeyCode hideKey = KeyCode.E; // Key to hide/unhide

    public bool IsHiding => isHiding; // Property to check if the player is hiding

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera
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
        mainCamera.transform.position = transform.position + cameraOffset; // Center camera on hiding spot
    }

    private void ExitHiding()
    {
        isHiding = false;
        playerSprite.enabled = true; // Show player sprite
        mainCamera.transform.position = player.transform.position + cameraOffset; // Return camera to player
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
            player = collision.gameObject;
            playerSprite = player.GetComponent<SpriteRenderer>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;

            if (isHiding)
            {
                ExitHiding();
            }
        }
    }
}
