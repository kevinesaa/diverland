using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ShopItem
{
    [System.Serializable]
    public enum TypeFromIAP
    {
        Consumable = 0,
        NonConsumable = 1,
        Subscription = 2
    }
    
    [HideInInspector]
    public int userQuantity;
    public abstract BaseItem BaseItem { get;}
    public bool isItemFromIAP;
    public int coins;
    public string idFromIAP;
    public TypeFromIAP typeFromIAP;
    public string PriceStringIAP { get; set; }
    public string Price { get { return isItemFromIAP ? PriceStringIAP : coins.ToString(); } }


    public bool Bought
    {
        get
        {
            return UserQuantity > 0 && 
                   !InventoryTypeItem.HAND.Equals(BaseItem.inventoryType) &&
                   !InventoryTypeItem.COIN.Equals(BaseItem.inventoryType);
        }
    }
    
    public int UserQuantity
    {
        get
        {
            if (userQuantity < 0 || InventoryTypeItem.COIN.Equals(BaseItem.inventoryType))
                userQuantity = 0;

            if (InventoryTypeItem.HAND.Equals(BaseItem.inventoryType))
                return userQuantity;

            if (userQuantity > 1)
                userQuantity = 1;

            return userQuantity;
        }

        set
        {
            int temp = value;
            if (temp < 0 || InventoryTypeItem.COIN.Equals(BaseItem.inventoryType))
                temp = 0;

            if (temp > 1 &&  !InventoryTypeItem.HAND.Equals(BaseItem.inventoryType))
                temp = 1;

            userQuantity = temp;
        }
    }

    
}
