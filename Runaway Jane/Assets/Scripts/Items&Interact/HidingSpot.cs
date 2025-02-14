using UnityEngine;

public class HidingSpot : MonoBehaviour
{
    private bool playerInside = false; // Is the player in range to hide?
    private GameObject player;
    private SpriteRenderer playerSprite;
    private Camera mainCamera;

    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Camera position offset
    [SerializeField] private KeyCode hideKey = KeyCode.E; // Key to hide/unhide
    [SerializeField] private Sprite closedWardrobeSprite; // The sprite for the closed wardrobe
    [SerializeField] private Sprite openWardrobeSprite; // The sprite for the open wardrobe

    private SpriteRenderer wardrobeSpriteRenderer;

    private bool isHiding = false; // Tracks if the player is hiding in the wardrobe
    private int spriteToggleCounter = 0; // Counter to handle sprite cycling

    public bool IsHiding { get; internal set; }

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera
        wardrobeSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer for wardrobe
    }

    private void Update()
    {
        if (playerInside && Input.GetKeyDown(hideKey))
        {
            // Cycle the sprite between closed ? opened ? closed
            spriteToggleCounter++;
            spriteToggleCounter %= 3; // Ensure it cycles between 0, 1, 2

            if (spriteToggleCounter == 0)
            {
                // Closed wardrobe
                wardrobeSpriteRenderer.sprite = closedWardrobeSprite;
                if (isHiding)
                {
                    UnhidePlayer();
                }
            }
            else if (spriteToggleCounter == 1)
            {
                // Open wardrobe
                wardrobeSpriteRenderer.sprite = openWardrobeSprite;
                if (!isHiding)
                {
                    HidePlayer();
                }
            }
            // No action needed for the 2nd cycle (closed), as it's handled by the 0 cycle

        }
    }

    private void HidePlayer()
    {
        // Player enters hiding state
        playerSprite.enabled = false; // Hide the player sprite
        mainCamera.transform.position = transform.position + cameraOffset; // Center camera on hiding spot
        isHiding = true;
    }

    private void UnhidePlayer()
    {
        // Player exits hiding state
        playerSprite.enabled = true; // Show the player sprite
        mainCamera.transform.position = player.transform.position + cameraOffset; // Return camera to player
        isHiding = false;
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
                // Ensure the player is visible when they exit the hiding spot
                wardrobeSpriteRenderer.sprite = closedWardrobeSprite;
                playerSprite.enabled = true;
                mainCamera.transform.position = player.transform.position + cameraOffset;
                isHiding = false;
            }
        }
    }
}
