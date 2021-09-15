using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDetail : MonoBehaviour {

    public Image image;
    public Text nameText;
    public Text descriptionText;

    private InventoryItem item;

    public void Setup(InventoryItem item)
    {
        this.item = item;
    }
}
