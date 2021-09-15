using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentServiceLocal : IEquipmentService
{
    public event Action<Equipment> OnUpdateEquipment;

    private Equipment equipment;

    public EquipmentServiceLocal()
    {
        LoadEquipment();
    }
    
    public void GetEquipment()
    {
        if (OnUpdateEquipment != null)
             OnUpdateEquipment(equipment);
    }

    public void EquipItem(InventoryItem item)
    {
        if (item.baseItem.inventoryType == InventoryTypeItem.HAND)
            UpdateHand(item);
        else
            Update(item.baseItem.inventoryType, item.baseItem.id);

        UpdateEquipment();
    }
    
    public void UnequipItem(InventoryItem item)
    {
        Update(item.baseItem.inventoryType,"");
        if (item.baseItem.inventoryType == InventoryTypeItem.HAND)
            equipment.handQuantity = 0;
        UpdateEquipment();
    }

    private void Update(InventoryTypeItem type, string id)
    {
        switch (type)
        {
            case InventoryTypeItem.BODY_PART1: equipment.bodyPart1 = id; break;
            case InventoryTypeItem.BODY_PART2: equipment.bodyPart2 = id; break;
            case InventoryTypeItem.BODY_PART3: equipment.bodyPart3 = id; break;
            case InventoryTypeItem.HAND: equipment.hand = id;  break;
        }
    }

    private void UpdateHand(InventoryItem item)
    {
        equipment.hand = item.baseItem.id;
        equipment.handQuantity = item.quantity;
    }

    private void LoadEquipment()
    {
        string json = PlayerPrefs.GetString(Constants.EQUIPMENT, "{}");
        equipment = JsonUtility.FromJson<Equipment>(json);
        if (equipment == null)
            equipment = new Equipment();
    }

    private void UpdateEquipment()
    {
        string json = JsonUtility.ToJson(equipment);
        PlayerPrefs.SetString(Constants.EQUIPMENT, json);
        PlayerPrefs.Save();
        if (OnUpdateEquipment != null)
            OnUpdateEquipment(equipment);
    }
}
