using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    public List<InventoryItem> items;

    public Inventory()
    {
        items = new List<InventoryItem>();
    }
}
