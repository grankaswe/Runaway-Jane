using UnityEngine;

public class Door : MonoBehaviour
{
    public string requiredKey = "Key"; // Name of the key needed to open the door
    private Inventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Finds the inventory component in the scene
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerInventory != null)
        {
            if (playerInventory.HasItem(requiredKey))
            {
                playerInventory.RemoveItem(requiredKey); // Remove key from inventory
                Destroy(gameObject); // Open the door
            }
        }
    }
}
