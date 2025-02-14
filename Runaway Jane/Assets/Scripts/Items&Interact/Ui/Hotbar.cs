using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [SerializeField] private int hotbarSize = 5; // Number of slots in hotbar
    [SerializeField] private List<Image> hotbarSlots; // UI slot images
    [SerializeField] private List<Text> quantityText; // UI Text elements to show quantities

    private List<InventoryItem> hotbarItems = new List<InventoryItem>(); // Items in hotbar
    private Inventory inventory;

    private void Start()
    {
        inventory = Inventory.Instance; // Get the inventory instance
        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateHotbar; // Listen for inventory updates
        }
        UpdateHotbar(); // Initialize hotbar UI
    }

    private void UpdateHotbar()
    {
        hotbarItems = new List<InventoryItem>(inventory.items); // Get items from the inventory
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            if (i < hotbarItems.Count)
            {
                hotbarSlots[i].enabled = true; // Show item
                hotbarSlots[i].sprite = GetItemSprite(hotbarItems[i].itemName); // Get item sprite
                quantityText[i].text = hotbarItems[i].quantity.ToString(); // Update quantity text
            }
            else
            {
                hotbarSlots[i].enabled = false; // Hide empty slots
                quantityText[i].text = ""; // Clear quantity text
            }
        }
    }

    private Sprite GetItemSprite(string itemName)
    {
        // Here you would implement logic to return an item sprite based on the item name
        // For simplicity, return a placeholder sprite
        return null; // Placeholder
    }

    // Example: Assign item to a specific slot in the hotbar
    public void AddToHotbar(string itemName)
    {
        // Ensure the hotbar is updated correctly
        InventoryItem item = inventory.items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            if (hotbarItems.Count < hotbarSize)
            {
                hotbarItems.Add(item);
                UpdateHotbar();
            }
        }
    }
}
