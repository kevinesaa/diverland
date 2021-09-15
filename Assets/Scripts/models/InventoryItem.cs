using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public BaseItemServer baseItem;
    public int quantity;

    public InventoryItem()
    {
        baseItem = new BaseItemServer();
    }

    public InventoryItem(ShopItem shopItem)
    {
        baseItem = new BaseItemServer();
        baseItem.id = shopItem.BaseItem.id;
        baseItem.name = shopItem.BaseItem.name;
        baseItem.description = shopItem.BaseItem.description;
        baseItem.inventoryType = shopItem.BaseItem.inventoryType;
        baseItem.SetImage(shopItem.BaseItem.Image);
    }
}
