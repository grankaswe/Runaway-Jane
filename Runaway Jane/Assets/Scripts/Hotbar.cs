using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Hotbar : MonoBehaviour
{
    public Inventory inventory;
    public Image[] hotbarSlots; // Array of Image components to display hotbar slots
    public int selectedHotbarSlot = 0; // The currently selected slot

    void Update()
    {
        // Switch hotbar slots with number keys (1-9)
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedHotbarSlot = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedHotbarSlot = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedHotbarSlot = 2;
        if (Input.GetKeyDown(KeyCode.Alpha4)) selectedHotbarSlot = 3;
        if (Input.GetKeyDown(KeyCode.Alpha5)) selectedHotbarSlot = 4;
        if (Input.GetKeyDown(KeyCode.Alpha6)) selectedHotbarSlot = 5;
        if (Input.GetKeyDown(KeyCode.Alpha7)) selectedHotbarSlot = 6;
        if (Input.GetKeyDown(KeyCode.Alpha8)) selectedHotbarSlot = 7;
        if (Input.GetKeyDown(KeyCode.Alpha9)) selectedHotbarSlot = 8;

        UpdateHotbarDisplay();
    }

    void UpdateHotbarDisplay()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i < inventory.inventoryItems.Count && inventory.inventoryItems[i] != null)
            {
                hotbarSlots[i].sprite = inventory.inventoryItems[i].itemIcon;
            }
            else
            {
                hotbarSlots[i].sprite = null;
            }
        }
    }
}
