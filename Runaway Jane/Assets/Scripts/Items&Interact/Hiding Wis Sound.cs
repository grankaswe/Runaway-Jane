using UnityEngine;
using System.Collections; // Needed for Coroutines

public class Hiding : MonoBehaviour
{
    private bool playerInside = false; // Is the player in range to hide?
    private bool isHiding = false; // Is the player currently hiding?
    private GameObject player;
    private SpriteRenderer playerSprite;
    private Camera mainCamera;
    public Sprite openSprite;   // Drag open wardrobe sprite in the Inspector
    public Sprite closedSprite; // Drag closed wardrobe sprite in the Inspector

    private SpriteRenderer spriteRenderer;
    private bool IsHiding = false; // Track if player is inside

    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10); // Camera offset
    [SerializeField] private KeyCode hideKey = KeyCode.E; // Key to hide/unhide
    [SerializeField] private bool useTrigger = true; // Toggle between collision or trigger

    // Audio variables
    [SerializeField] private AudioClip doorOpenSound; // Drag the door opening sound in the Inspector
    [SerializeField] private AudioClip doorCloseSound; // Drag the door closing sound in the Inspector
    private AudioSource audioSource; // Reference to the AudioSource component

    private void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = closedSprite; // Start with closed wardrobe

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
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
        if (!IsHiding) // Prevent re-entering
        {
            IsHiding = true;
            StartCoroutine(OpenAndCloseWardrobe()); // Show open briefly, then close
        }
    }

    private void ExitHiding()
    {
        isHiding = false;
        playerSprite.enabled = true; // Show player sprite
        mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z); // Reset camera
        if (IsHiding) // Prevent exiting if not inside
        {
            IsHiding = false;
            StartCoroutine(OpenAndCloseWardrobe()); // Show open briefly, then close
        }
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

    private IEnumerator OpenAndCloseWardrobe()
    {
        // Play door opening sound
        if (doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }

        spriteRenderer.sprite = openSprite; // Show open wardrobe
        yield return new WaitForSeconds(0.2f); // Wait for animation effect

        // Play door closing sound
        if (doorCloseSound != null)
        {
            audioSource.PlayOneShot(doorCloseSound);
        }

        spriteRenderer.sprite = closedSprite; // Switch back to closed
    }
}
