using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopCategoryLocal : ShopCategory
{
    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private List<ShopItemLocal> shopItemsLocals;

    private List<ShopItem> shopItems;
    
    public override List<ShopItem> ShopItems
    {
        get
        {
            if(shopItems==null)
            {
                if (shopItemsLocals == null)
                    return null;
                else
                {
                    shopItems = new List<ShopItem>();
                    foreach (ShopItemLocal item in shopItemsLocals)
                        shopItems.Add(item);
                        
                    return shopItems;
                }
            }
            else
            {
                return shopItems;
            }
        }
    }

    public List<ShopItemLocal> CategoryLocals
    {
        get { return shopItemsLocals; }

        set
        {
            this.shopItemsLocals = value;
            this.shopItems = new List<ShopItem>();
            foreach (ShopItem category in shopItemsLocals)
            {
                shopItems.Add(category);
            }
        }
    }

    public override Sprite Sprite
    {
        get
        {
            return sprite;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }
}
