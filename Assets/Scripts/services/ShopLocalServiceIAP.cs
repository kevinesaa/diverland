using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLocalServiceIAP : IShopServiceIAP
{
    public event Action<ShopItem> OnPurchaseSuccessful;
    public event Action<string, string> OnPurchaseFail;

    public void Buy(string user, ShopItem shopItem)
    {
        int totalCoins = GetCoins();
        totalCoins += shopItem.coins;
        PlayerPrefs.SetInt(Constants.COIN_SAVED, totalCoins);
        if (OnPurchaseSuccessful != null)
            OnPurchaseSuccessful(shopItem);
        
    }

    private int GetCoins()
    {
        return PlayerPrefs.GetInt(Constants.COIN_SAVED, 0);
    }
}
