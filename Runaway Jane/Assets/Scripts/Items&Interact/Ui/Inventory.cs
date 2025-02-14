using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public static Inventory Instance;

    // List of items in the player's inventory
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxSlots = 10;

    // The hotbar UI slots, mapped from UXML
    public VisualElement[] hotbarSlots;

    public event System.Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        // Initialize the UI system for the hotbar (referencing slots from the UXML file)
        SetupUI();
    }

    private void SetupUI()
    {
        // Get the UI root and map the hotbar slots
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        hotbarSlots = new VisualElement[]
        {
            root.Q<VisualElement>("slot1"),
            root.Q<VisualElement>("slot2"),
            root.Q<VisualElement>("slot3"),
            root.Q<VisualElement>("slot4"),
            root.Q<VisualElement>("slot5")
        };

        // Update the hotbar initially
        UpdateHotbar();
    }

    // Update the hotbar UI based on the inventory
    public void UpdateHotbar()
    {
        // Loop through all hotbar slots and update them
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i < items.Count)
            {
                InventoryItem item = items[i];
                var slot = hotbarSlots[i];

                // Set the slot's display to the item name (or you can add an image/icon)
                slot.Clear(); // Clear previous content if necessary

                // Display item info as text (this can be expanded with icons or images)
                var label = new Label($"{item.itemName} x{item.quantity}");
                slot.Add(label);
            }
            else
            {
                // If there is no item in this slot, you can set a placeholder or leave it empty
                hotbarSlots[i].Clear();
            }
        }
    }

    // Add an item to the inventory
    public bool AddItem(string itemName, int quantity = 1)
    {
        InventoryItem existingItem = items.Find(item => item.itemName == itemName);

        if (existingItem != null)
        {
            existingItem.quantity += quantity;
        }
        else if (items.Count < maxSlots)
        {
            items.Add(new InventoryItem(itemName, quantity));
        }

        OnInventoryChanged?.Invoke();
        UpdateHotbar(); // Update the UI whenever an item is added
        return true;
    }

    // Remove an item from the inventory
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
            OnInventoryChanged?.Invoke();
            UpdateHotbar(); // Update the UI whenever an item is removed
            return true;
        }
        return false;
    }

    // Check if the inventory has a specific item
    public bool HasItem(string itemName)
    {
        return items.Exists(item => item.itemName == itemName);
    }
}
