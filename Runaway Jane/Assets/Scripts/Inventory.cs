using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public int inventorySize = 10; // Number of items the inventory can hold
    public List<Item> inventoryItems = new List<Item>();

    void Start()
    {
        // Initialize the inventory with empty slots
        for (int i = 0; i < inventorySize; i++)
        {
            inventoryItems.Add(null);
        }
    }

    // Method to add items to the inventory
    public bool AddItem(Item item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i] == null)
            {
                inventoryItems[i] = item;
                return true;
            }
        }
        return false; // No empty slots
    }

    // Method to remove items
    public void RemoveItem(Item item)
    {
        inventoryItems.Remove(item);
        inventoryItems.Add(null); // Add null to the list to keep the size constant
    }
}

