using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectedItemHolder : MonoBehaviour
{
    public Image image;
    public Text nameText;
    public Text descriptionText;
    public Button equipItemButton;
    public Button unequipItemButton;
    public Sprite emptyImage;

    public InventoryController Inventory { get; set; }

    private InventoryItem item;
    private bool equipment = false;

    private void Start()
    {
        nameText.text = "";
        descriptionText.text = "";
        equipItemButton.onClick.AddListener(EquipItem);
        unequipItemButton.onClick.AddListener(UnequipItem);
    }

    public void Show(InventoryItem item,bool equipment)
    {
        this.item = item;
        this.equipment = equipment;
        image.sprite = item.baseItem.Image;
        nameText.text = item.baseItem.name;
        descriptionText.text = item.baseItem.description;
        equipItemButton.gameObject.SetActive(!equipment);
        unequipItemButton.gameObject.SetActive(equipment);
    }

    public void Clear()
    {
        image.sprite = emptyImage;
        nameText.text = "";
        descriptionText.text = "";
        equipItemButton.gameObject.SetActive(false);
        unequipItemButton.gameObject.SetActive(false);
    }

    private void EquipItem()
    {
        Inventory.EquipItem(item);
    }

    private void UnequipItem()
    {
        Inventory.UnequipItem(item);
    }
}
