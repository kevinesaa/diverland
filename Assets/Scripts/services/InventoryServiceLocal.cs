using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryServiceLocal : IInventoryService
{
    public event Action<Inventory> OnLoadInventoryDataSuccessfully;
    public event Action<string, string> OnLoadInventoryDataFail;

    private Inventory inventory;

    public InventoryServiceLocal()
    {
        inventory = new Inventory();
        string jsonInventory = PlayerPrefs.GetString(Constants.INVENTORY, "{}");
        inventory = JsonUtility.FromJson<Inventory>(jsonInventory);
    }

    public void GetInventoryData(string user)
    {
        if (OnLoadInventoryDataSuccessfully != null)
            OnLoadInventoryDataSuccessfully(inventory);
    }
}
