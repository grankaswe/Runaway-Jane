using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Items : MonoBehaviour
{
    public string itemName;
    public Sprite itemIcon;

    // Constructor
    public Items(string name, Sprite icon)
    {
        itemName = name;
        itemIcon = icon;
    }
}
