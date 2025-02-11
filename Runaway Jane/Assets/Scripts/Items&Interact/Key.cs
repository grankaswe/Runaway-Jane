using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private string keyName = "Key"; // Set key name in the Inspector
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Interaction key
    private Inventory playerInventory;
    private bool playerNearby = false;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Find inventory in the scene
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            if (playerInventory != null && playerInventory.AddItem(keyName))
            {
                Debug.Log($"Picked up {keyName}");
                Destroy(gameObject); // Remove key from the scene
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player enters trigger zone
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player leaves trigger zone
        {
            playerNearby = false;
        }
    }
}
