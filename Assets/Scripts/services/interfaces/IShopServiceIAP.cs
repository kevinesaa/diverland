using System;
using System.Collections.Generic;
using UnityEngine;

public interface IShopServiceIAP
{
    event Action<ShopItem> OnPurchaseSuccessful;
    event Action<string, string> OnPurchaseFail;
    void Buy(string user, ShopItem shopItem);
}
