using System;

public interface IBuyShopItemService
{
    event Action<int, ShopItem> OnPurchaseSuccessful;
    event Action<string, string> OnPurchaseFail;
    void BuyItem(string user, ShopItem item);
}
