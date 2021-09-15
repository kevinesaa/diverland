using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IButtonHolder<T> : MonoBehaviour
{
    public T Data { get; set; }
    public ShopController Shop { get; set; }

    private void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(Show);
    }

    public abstract void Setup(T data, ShopController shop);
    public abstract void Show();
}
