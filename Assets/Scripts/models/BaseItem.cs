
using UnityEngine;

[System.Serializable]
public abstract class BaseItem 
{
    public string name;
    public string id;
    public string description;
    public InventoryTypeItem inventoryType;
    public abstract Sprite Image { get; }
}
