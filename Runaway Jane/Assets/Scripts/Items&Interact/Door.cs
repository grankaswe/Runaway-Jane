using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string requiredKey = "Key"; // Key required to open the door
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to interact with the door
    [SerializeField] private Sprite openDoorSprite; // Assign the open door sprite in the Inspector
    private SpriteRenderer spriteRenderer;
    private Collider2D doorCollider;
    private Inventory playerInventory;
    private bool playerNearby = false;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Finds the player's inventory
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer
        doorCollider = GetComponent<Collider2D>(); // Get the collider
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(interactKey)) // Only opens when E is pressed
        {
            TryToOpenDoor();
        }
    }

    private void TryToOpenDoor()
    {
        if (playerInventory != null && playerInventory.HasItem(requiredKey)) // Check if player has key
        {
            playerInventory.RemoveItem(requiredKey); // Use key
            OpenDoor();
        }
        else
        {
            Debug.Log("Door is locked! You need a " + requiredKey);
        }
    }

    private void OpenDoor()
    {
        if (openDoorSprite != null)
        {
            spriteRenderer.sprite = openDoorSprite; // Change to open door sprite
        }

        if (doorCollider != null)
        {
            doorCollider.enabled = false; // Disable the collider to make it walk-through
        }

        Debug.Log("Door unlocked!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
