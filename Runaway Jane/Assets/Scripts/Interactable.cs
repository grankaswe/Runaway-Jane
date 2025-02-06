using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public string itemName = "Key";  // Item inside the object
    private bool isPlayerNearby = false;

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E)) // Press "E" to interact
        {
            Interact();
        }
    }

    void Interact()
    {
        Inventory inventory = FindObjectOfType<Inventory>(); // Find the player's inventory script
        if (inventory != null)
        {
            bool added = inventory.AddItem(itemName); // Try adding item to inventory
            if (added)
            {
                Debug.Log("Picked up: " + itemName);
                Destroy(gameObject); // Remove the object after picking up the item
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure the player has the tag "Player"
        {
            isPlayerNearby = true;
            Debug.Log("Press 'E' to interact.");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
        }
    }
}
