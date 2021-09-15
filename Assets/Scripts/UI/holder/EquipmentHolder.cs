using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentHolder : MonoBehaviour {

    public Image bodyPart1Image;
    public Image bodyPart2Image;
    public Image bodyPart3Image;
    public Image handImage;
    public Text quantityHandItemText;
    public Sprite defaultEmptyItem;

    private IDictionary<InventoryTypeItem, InventoryItem> equiment;

    public void Set(IDictionary<InventoryTypeItem,InventoryItem> equiment)
    {
        this.equiment = equiment;
        Set(InventoryTypeItem.BODY_PART1, bodyPart1Image);
        Set(InventoryTypeItem.BODY_PART2, bodyPart2Image);
        Set(InventoryTypeItem.BODY_PART3, bodyPart3Image);
        SetHand();
    }

    private void Set(InventoryTypeItem type, Image image)
    {
        Sprite sprite = defaultEmptyItem;
        InventoryItem item = null;
        if (equiment.ContainsKey(type))
            item = equiment[type];
        
        if (item != null && item.quantity > 0)
            sprite = item.baseItem.Image;

        image.sprite = sprite;
    }

    private void SetHand()
    {
        if (equiment.ContainsKey(InventoryTypeItem.HAND))
        {
            InventoryItem item = equiment[InventoryTypeItem.HAND];
            if (item != null && item.quantity > 0)
            {
                quantityHandItemText.text = item.quantity.ToString();
            }
            else
            {
                quantityHandItemText.text = "";
            }
        }
        Set(InventoryTypeItem.HAND, handImage);
    }
}
