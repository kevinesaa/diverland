using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItemLocal : ShopItem
{
    [SerializeField]
    private BaseItemLocal baseItem;

    public override BaseItem BaseItem
    {
        get
        {
            return baseItem;
        }
    }

    public BaseItemLocal BaseItemLocal { get { return baseItem; } set { baseItem = value; } }
}
