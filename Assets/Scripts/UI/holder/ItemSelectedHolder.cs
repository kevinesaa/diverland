using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectedHolder : MonoBehaviour
{
    public Text nameText;
    public Image image;
    public Text description;
    public Text price;
    public GameObject coinImage;
    public GameObject realMoneyImage;

    private ShopItem data;

    public void Setup(ShopItem item)
    {
        this.data = item;
        nameText.text = data.BaseItem.name;
        image.sprite = data.BaseItem.Image;
        image.preserveAspect = true;
        description.text = data.BaseItem.description;
        price.text = data.Price;
        coinImage.SetActive(!data.isItemFromIAP);
        realMoneyImage.SetActive(data.isItemFromIAP);
    }

    public void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
