using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string requiredKey = "Key"; // Key required to open the door
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to interact with the door
    private Inventory playerInventory;
    private bool playerNearby = false;
    private GameObject player;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Finds the player's inventory
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
            Destroy(gameObject); // Remove door
        }
        else
        {
            Debug.Log("Door is locked! You need a " + requiredKey); // Optional: Display message in console/UI
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerNearby = true;
            player = collision.gameObject;
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
