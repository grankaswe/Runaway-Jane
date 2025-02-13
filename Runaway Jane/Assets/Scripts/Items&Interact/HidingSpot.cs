using UnityEngine;
using System.Collections;

public class HidingSpot : MonoBehaviour
{
    private bool playerInside = false;
    public bool IsHiding { get; private set; } = false; // Public getter for enemy AI
    private GameObject player;
    private Camera mainCamera;
    public Sprite openSprite;
    public Sprite closedSprite;

    private SpriteRenderer spriteRenderer;
    private Collider2D wardrobeCollider;
    private Collider2D reachCollider;

    [SerializeField] private Vector2 cameraOffset = new Vector2(0, -10);
    [SerializeField] private KeyCode hideKey = KeyCode.E;
    [SerializeField] private bool useTrigger = true;

    private void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        wardrobeCollider = GetComponent<Collider2D>();
        reachCollider = transform.Find("reach").GetComponent<Collider2D>();  // Find the 'reach' GameObject and its collider
        spriteRenderer.sprite = closedSprite;
    }

    private void Update()
    {
        // Ensure the wardrobe is active for interaction
        if (!gameObject.activeInHierarchy)
            return;

        // Only allow hiding when the player is inside the reach area
        if (playerInside && Input.GetKeyDown(hideKey))
        {
            if (!IsHiding)
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

        IsHiding = true;

        // Disable all renderers on the player to make them fully invisible
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        // Disable wardrobe's visual components while keeping it active for interaction
        spriteRenderer.enabled = false;
        wardrobeCollider.enabled = false; // Optionally, disable the collider to prevent further triggers

        // Move camera to the hiding spot
        mainCamera.transform.position = new Vector2(transform.position.x, mainCamera.transform.position.y);

        StartCoroutine(OpenAndCloseWardrobe());
    }

    private void ExitHiding()
    {
        IsHiding = false;

        // Re-enable all renderers on the player
        Renderer[] renderers = player.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }

        // Re-enable wardrobe's visual components
        spriteRenderer.enabled = true;
        wardrobeCollider.enabled = true; // Re-enable the collider to allow further interactions

        // Move camera back to the player's position
        mainCamera.transform.position = new Vector2(player.transform.position.x, mainCamera.transform.position.y);

        StartCoroutine(OpenAndCloseWardrobe());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only trigger when the player enters the 'reach' collider area
        if (collision == reachCollider && collision.gameObject.CompareTag("Player"))
        {
            playerInside = true;
            player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Only reset when the player exits the 'reach' collider area
        if (collision == reachCollider && collision.gameObject.CompareTag("Player"))
        {
            playerInside = false;
            if (IsHiding) ExitHiding();
        }
    }

    private IEnumerator OpenAndCloseWardrobe()
    {
        spriteRenderer.sprite = openSprite;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = closedSprite;
    }
}
