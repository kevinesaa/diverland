using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ShopCategory
{
    public string name;
    public string id;
    public abstract Sprite Sprite{get;}
    public abstract List<ShopItem> ShopItems { get; }
}
