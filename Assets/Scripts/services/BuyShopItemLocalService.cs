using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyShopItemLocalService : IBuyShopItemService
{
    public event Action<int, ShopItem> OnPurchaseSuccessful;
    public event Action<string, string> OnPurchaseFail;

    private Inventory inventory;
    private IDictionary<string, InventoryItem> inventoryDictionary;

    public BuyShopItemLocalService()
    {
        LoadInventory();
    }

    public void BuyItem(string user, ShopItem item)
    {
        int totalCoins = GetCoins();
        totalCoins -= item.coins;
        PlayerPrefs.SetInt(Constants.COIN_SAVED, totalCoins);

        if (!inventoryDictionary.ContainsKey(item.BaseItem.id))
        {
            AddItemToInventory(item);
        }

        UpdateInventory(item);
        SaveInventory();
        
        if (OnPurchaseSuccessful != null)
            OnPurchaseSuccessful(totalCoins, item);
    }

    private int GetCoins()
    {
        return PlayerPrefs.GetInt(Constants.COIN_SAVED, 0);
    }

    private void LoadInventory()
    {
        string jsonInventory = PlayerPrefs.GetString(Constants.INVENTORY, "{}");
        inventory = JsonUtility.FromJson<Inventory>(jsonInventory);
        if (inventory == null)
            inventory = new Inventory();

        inventoryDictionary = new Dictionary<string, InventoryItem>();

        foreach (InventoryItem it in inventory.items)
        {
            inventoryDictionary[it.baseItem.id] = it;
        }
    }

    private void AddItemToInventory(ShopItem item)
    {
        InventoryItem inventoryItem = new InventoryItem(item);
        inventoryDictionary.Add(item.BaseItem.id, inventoryItem);
        inventoryItem.quantity = 1;
        item.UserQuantity = 1;
    }

    private void UpdateInventory(ShopItem item)
    {
        InventoryItem inventoryItem = inventoryDictionary[item.BaseItem.id];
        if (inventoryItem.baseItem.inventoryType.Equals(InventoryTypeItem.HAND))
        {
            if (inventoryItem.quantity < 0)
                inventoryItem.quantity = 0;
            inventoryItem.quantity++;
            item.UserQuantity = inventoryItem.quantity;
        }
        inventory.items.Add(inventoryItem);
    }

    private void SaveInventory()
    {
        string jsonString = JsonUtility.ToJson(inventory);
        PlayerPrefs.SetString(Constants.INVENTORY, jsonString);
        PlayerPrefs.Save();
    }
}
