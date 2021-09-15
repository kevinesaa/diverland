using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButtonHolder : IButtonHolder<ShopItem>
{
    public Text titleText;
    public Text countText;
    public Image itemImage;
    public Text priceText;
    public Image coinImage;
    public Image currencyImage;
    public Image playerHavenImage;

    public override void Setup(ShopItem data, ShopController shop)
    {
        Data = data;
        Shop = shop;
        titleText.text = Data.BaseItem.name;
        itemImage.sprite = Data.BaseItem.Image;
        itemImage.preserveAspect = true;
        priceText.text = Data.Price;
        coinImage.gameObject.SetActive(!Data.isItemFromIAP);
        currencyImage.gameObject.SetActive(Data.isItemFromIAP);
        playerHavenImage.gameObject.SetActive(Data.Bought);
        countText.text = Data.UserQuantity > 1 ? Data.UserQuantity.ToString(): "";
    }

    public override void Show()
    {
        Shop.ShowItem(Data);
    }
}
