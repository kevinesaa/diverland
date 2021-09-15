using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductListLocalService :  IProductListService
{
    
    public event Action<IList<ShopCategory>> OnShopDataLoadSuccessful;
    public event Action<string, string> OnShopDataLoadedFail;

    private IList<ShopCategory> shopCategories;

    public ProductListLocalService(IList<ShopCategory> shopCategories)
    {
        this.shopCategories = shopCategories;
    }

    public ProductListLocalService(IList<ShopCategoryLocal> shopCategoriesLocal)
    {
        this.shopCategories = new List<ShopCategory>();
        foreach (ShopCategoryLocal category in shopCategoriesLocal)
        {
            this.shopCategories.Add(category);
        }
    }

    public void GetShopItems(string user)
    {
        Inventory inventory = UserInventory();

        foreach(InventoryItem inventoryItem in inventory.items)
        {
            foreach(ShopCategory category in shopCategories)
            {
                bool again = true;
                foreach(ShopItem shopItem in category.ShopItems)
                {
                    if (inventoryItem.baseItem.id.Equals(shopItem.BaseItem.id))
                    {
                        again = false;
                        shopItem.UserQuantity = inventoryItem.quantity;
                        break;
                    }
                }
                if (!again)
                    break;
            }
        }
        if (OnShopDataLoadSuccessful != null)
            OnShopDataLoadSuccessful(shopCategories);
    }


    private Inventory UserInventory()
    {
        string jsonString = PlayerPrefs.GetString(Constants.INVENTORY, "{}");
        return JsonUtility.FromJson<Inventory>(jsonString);
    }
}
