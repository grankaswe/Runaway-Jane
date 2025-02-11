using UnityEngine;
using System.Collections.Generic;

public class Furniture : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E; // Key to loot
    [SerializeField] private bool destroyAfterLooting = false; // Should furniture disappear after looting?

    [System.Serializable]
    public class LootItem
    {
        public string itemName;
        public int minAmount = 1;
        public int maxAmount = 3;
    }

    [SerializeField] private List<LootItem> possibleLoot = new List<LootItem>(); // List of possible loot

    private Inventory playerInventory;
    private bool playerNearby = false;

    private void Start()
    {
        playerInventory = FindObjectOfType<Inventory>(); // Find the player's inventory in the scene
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            LootFurniture();
        }
    }

    private void LootFurniture()
    {
        if (playerInventory != null && possibleLoot.Count > 0)
        {
            foreach (LootItem loot in possibleLoot)
            {
                int amount = Random.Range(loot.minAmount, loot.maxAmount + 1); // Randomized loot quantity
                bool added = playerInventory.AddItem(loot.itemName, amount);

                if (added)
                {
                    Debug.Log("You looted " + amount + " " + loot.itemName);
                }
                else
                {
                    Debug.Log("Inventory is full! Could not take " + loot.itemName);
                }
            }

            if (destroyAfterLooting)
            {
                Destroy(gameObject); // Remove furniture after looting
            }
        }
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
