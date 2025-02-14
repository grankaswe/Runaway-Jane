using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]

public class InventoryItem
{
    public string itemName;
    public int quantity;

    public InventoryItem(string name, int qty = 1)
    {
        itemName = name;
        quantity = qty;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxSlots = 10;

    public bool AddItem(string itemName, int quantity = 1)
    {
        InventoryItem existingItem = items.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
            return true;
        }
        else if (items.Count < maxSlots)
        {
            items.Add(new InventoryItem(itemName, quantity));
            return true;
        }

        return false; // Inventory full
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        InventoryItem existingItem = items.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity -= quantity;
            if (existingItem.quantity <= 0)
            {
                items.Remove(existingItem);
            }
            return true;
        }

        return false; // Item not found
    }

    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI; // Assign in Inspector
    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Press "I" to toggle inventory
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryUI.SetActive(isOpen);
    }
}

}
