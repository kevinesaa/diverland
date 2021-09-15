using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonHolder : IButtonHolder<ShopCategory>
{
    public Image image;
    public Text categoryName;

    public override void Setup(ShopCategory data, ShopController shop)
    {
        Data = data;
        Shop = shop;
        image.sprite = Data.Sprite;
        image.preserveAspect = true;
        categoryName.text = Data.name;
    }

    public override void Show()
    {
        Shop.ShowItemsFromCategory(Data);
    }
}
