using System;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipmentService
{
    event Action<Equipment> OnUpdateEquipment;
    void GetEquipment();
    void EquipItem(InventoryItem item);
    void UnequipItem(InventoryItem item);
}
