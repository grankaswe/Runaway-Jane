using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField] private string keyName = "Key"; // Set key name in the Inspector
    private Inventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Finds the inventory component in the scene
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerInventory != null)
        {
            if (playerInventory.AddItem(keyName)) // Adds key to inventory
            {
                Destroy(gameObject); // Remove key from scene if successfully added
            }
        }
    }
}
