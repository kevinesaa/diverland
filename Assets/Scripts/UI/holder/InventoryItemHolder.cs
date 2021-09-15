using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemHolder : MonoBehaviour {

    public Image imageEquip;
    public Image image;
    public Text nameText;
    public Text quantityText;

    private InventoryItem item;
    private InventoryController inventory;
    private bool equipment = false;

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Show);
    }

    public void Setup(InventoryItem item, InventoryController inventory, bool equipment)
    {
        this.item = item;
        this.inventory = inventory;
        this.equipment = equipment;
        image.sprite = this.item.baseItem.Image;
        nameText.text = this.item.baseItem.name;
        imageEquip.color = equipment ? Color.green : Color.white;
        quantityText.text = "";
        if (InventoryTypeItem.HAND.Equals(item.baseItem.inventoryType))
        {
            quantityText.text = item.quantity.ToString();
        }
    }

    private void Show()
    {
        inventory.ShowItem(item);
    }
}
